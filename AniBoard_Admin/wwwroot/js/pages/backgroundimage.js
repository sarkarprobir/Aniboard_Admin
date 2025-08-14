function searchbackgroundimage() {
    var searchText = $('#backgroundimage_search').val();
    if (searchText != null && searchText != '') {
        window.location.href = '/Element/BackgroundImage?q=' + searchText;
    }
    else {
        window.location.href = '/Element/BackgroundImage';
    }
}

function showaddeditbackgroundimage_modal(imageId) {
    $("#backgroundimage_body").empty();

    $.ajax({
        url: "/Element/ShowAddEditBackgroundImageModal",
        type: "GET",
        data: { "imageId": imageId },
        datatype: "json",
        success: function (data) {
            $("#backgroundimage_body").html(data);
        },
    });
}