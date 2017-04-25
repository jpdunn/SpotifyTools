Imports SpotifyAPI.Web
Imports SpotifyAPI.Web.Auth
Imports SpotifyAPI.Web.Enums
Imports SpotifyAPI.Web.Models


''' <summary>
''' Defines the data context for the main window.
''' </summary>
Public Class MainWindowDataContext

    Private Property cgSpotifyAPI As SpotifyWebAPI


    Public Sub New()
        Login = New RelayCommand(Sub() PerformLogin())
    End Sub


#Region "Public Properties"

    ''' <summary>
    ''' Gets the command to log in to spotify.
    ''' </summary>
    ''' <returns>The login command.</returns>
    Public ReadOnly Property Login As ICommand

#End Region


    ''' <summary>
    ''' Runs an async task to log the user into Spotify.
    ''' </summary>
    Private Sub PerformLogin()
        Task.Run(Sub() AuthenticateUser())
    End Sub


    ''' <summary>
    ''' Authenticates the user and if successful, stores the web API into the class global.
    ''' </summary>
    Private Async Sub AuthenticateUser()
        Dim webApiFactory As New WebAPIFactory(
            "http://localhost",
            8000,
            "26d287105e31491889f3cd293d85bfea",
            Scope.UserReadPrivate Or
            Scope.UserReadEmail Or
            Scope.UserFollowRead Or
            Scope.UserTopRead
        )

        Try
            cgSpotifyAPI = Await webApiFactory.GetWebApi()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub


    ''' <summary>
    ''' Gets the artists that are followed by the current user.
    ''' </summary>
    ''' <returns>A <see cref="FollowedArtists"/> object containing the artists that are followed by the current user.</returns>
    Private Function GetFollowedArtists() As FollowedArtists
        Dim artists As FollowedArtists


        ' TODO: Currently the API we are using only supports a maximum of 50 followed artists per request, so for now
        ' just limit it to 50 and we can expand it once we get full functionality of this application.
        artists = cgSpotifyAPI.GetFollowedArtists(FollowType.Artist, 50)

        Return artists
    End Function


    ''' <summary>
    ''' Gets the latest release (Single or Album) for a given artist.
    ''' </summary>
    ''' <param name="artistID">The ID of the artist to get data for.</param>
    ''' <returns>A <see cref="SimpleAlbum"/> object containing the relevant data.</returns>
    Private Function GetArtistLatestRelease(artistID As String) As SimpleAlbum
        Dim results As Paging(Of SimpleAlbum)


        results = cgSpotifyAPI.GetArtistsAlbums(artistID, AlbumType.Album Or AlbumType.Single, 1)

        Return results.Items.FirstOrDefault()
    End Function

End Class
