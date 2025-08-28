function searchbackgroundimage() {
    var searchText = $('#backgroundimage_search').val();
    if (searchText != null && searchText != '') {
        window.location.href = '../Element/BackgroundImage?q=' + searchText;
    }
    else {
        window.location.href = '../Element/BackgroundImage';
    }
}

function showaddeditbackgroundimage_modal(imageId) {
    $("#backgroundimage_body").empty();

    $.ajax({
        url: "../Element/ShowAddEditBackgroundImageModal",
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
            url: "../Element/ShowBackgroundImagelist",
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


function deletebackgroundimage(imageId) {
    if (confirm("Are you sure you want to delete this background Image?")) {
        var searchText = $('#backgroundimage_search').val();
        const formData = new FormData();
        //formData.append("file", fileInput.files[0]);
        formData.append("imageId", imageId);
        formData.append("isDelete", 1);

        //uploadStatus.textContent = "Uploading...";

        fetch("../Element/SaveBackgroundImage", {
            method: "POST",
            body: formData
        })
            .then(response => {
                if (!response.ok) {
                    alert(response.errmsg);
                }
                return response.json();
            })
            .then(data => {
                
                alert('Background image deleted sucessfully');
                
                setTimeout(function () {
                    if (searchText != null && searchText != '') {
                        window.location.href = '../Element/BackgroundImage?q=' + searchText;
                    }
                    else {
                        window.location.href = '../Element/BackgroundImage';
                    }

                }, 1000);
            })
            .catch(err => {
                alert(err.message);
                //uploadStatus.textContent = "Error: " + err.message;
            });
    }


    return false;


}
