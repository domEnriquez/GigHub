var GigDetailsController = function (followService) {
    var button;

    var init = function (container) {
        $(container).on("click", ".js-toggle-follow", toggleFollow);
    };

    var toggleFollow = function (e) {
        button = $(e.target);
        var artistId = button.attr("data-user-id");

        if (button.hasClass("btn-default"))
            followService.createFollpowing(artistId, done, fail);
        else
            followService.deleteFollowing(artistId, done, fail);
    };

    var fail = function () {
        alert("Something failed!");
    };

    var done = function () {
        var text = (button.text() == "Following") ? "Follow" : "Following";
        button.toggleClass("btn-info").toggleClass("btn-default").text(text);
    };

    return {
        init: init
    };


}(FollowService);