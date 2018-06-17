var FollowService = function () {
    var createFollpowing = function (artistId, done, fail) {
        $.post("/api/follow", { followeeId: artistId })
            .done(done)
            .fail(fail);
    };

    var deleteFollowing = function (artistId, done, fail) {
        $.ajax({
            url: "/api/follow/" + artistId,
            method: "DELETE"
        })
            .done(done)
            .fail(fail);
    };

    return {
        createFollpowing: createFollpowing,
        deleteFollowing: deleteFollowing
    };
}();