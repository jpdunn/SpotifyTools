Imports SpotifyAPI.Web
Imports SpotifyAPI.Web.Auth
Imports SpotifyAPI.Web.Enums
Imports SpotifyAPI.Web.Models


Class MainWindow

    Private cgSpotifyAPI As SpotifyWebAPI


    Private Sub button_Click(
            sender As Object,
            e As RoutedEventArgs
        ) Handles button.Click

        Task.Run(Sub() RunAuthentication())
    End Sub


    Private Async Sub RunAuthentication()
        Dim followedArtists As FollowedArtists
        Dim albumIDs As List(Of SimpleAlbum)
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

        If cgSpotifyAPI Is Nothing Then
            Return
        End If

        ' TODO: Once auth is done begin application writing stuff...

        followedArtists = GetFollowedArtists()
        albumIDs = New List(Of SimpleAlbum)

        For Each artist In followedArtists.Artists.Items
            albumIDs.Add(GetArtistLatestRelease(artist.Id))
        Next artist
    End Sub


    Private Function GetFollowedArtists() As FollowedArtists
        Dim artists As FollowedArtists


        artists = cgSpotifyAPI.GetFollowedArtists(FollowType.Artist, 50)

        Return artists
    End Function


    Private Function GetArtistLatestRelease(artistID As String) As SimpleAlbum
        Dim results As Paging(Of SimpleAlbum)


        results = cgSpotifyAPI.GetArtistsAlbums(artistID, AlbumType.Album Or AlbumType.Single, 1)

        Return results.Items.FirstOrDefault()
    End Function

End Class
