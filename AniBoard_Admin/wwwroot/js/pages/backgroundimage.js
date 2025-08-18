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

function gotopage(pageno) {
    var searchText = $('#backgroundimage_search').val();
    if (pageno > 0) {
        
        $.ajax({
            url: "/Element/ShowBackgroundImagelist",
            type: "GET",
            data: {
                "q": searchText,
                "pageNo": pageno
            },
            datatype: "json",
            success: function (data) {
                $("#backgroundimagelisttable").empty();
                $("#backgroundimagelisttable").html(data);
                document.querySelectorAll(".page-item").forEach(li => li.classList.remove("active"));
                let activeLi = document.getElementById('page_' + pageno);
                if (activeLi) {
                    activeLi.classList.add("active");
                }
            },
        });
    }

}
function pageInput() {
    var pageno = $('#userPageInput').val();
    //alert(pageno);
    if (pageno > 0) {
        gotopage(pageno);
    }

}
