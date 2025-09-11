function searchelement() {
    var searchText = $('#element_search').val();
    if (searchText != null && searchText != '') {
        window.location.href = '/Element/Elements?q=' + searchText;
    }
    else {
        window.location.href = '/Element/Elements';
    }
}

function showaddeditelement_modal(elementId) {
    $("#element_body").empty();

    $.ajax({
        url: "/Element/ShowAddEditElementModal",
        type: "GET",
        data: { "elementId": elementId },
        datatype: "json", 
        success: function (data) {
            $("#element_body").html(data);
        },
    });
}

function gotopage(pageno) {
    var searchText = $('#element_search').val();
    if (pageno > 0) {

        $.ajax({
            url: "/Element/ShowElementlist",
            type: "GET",
            data: {
                "q": searchText,
                "pageNo": pageno
            },
            datatype: "json",
            success: function (data) {
                $("#elementlisttable").empty();
                $("#elementlisttable").html(data);
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
    var totalCount = $('#hd_totalCount').val();
    //alert(pageno);
    if (pageno > totalCount)
    {
        pageno = totalCount;
    }
    if (pageno > 0) {
        gotopage(pageno);
    }

}

function deleteelement(elementId) {
    if (confirm("Are you sure you want to delete this element?")) {
        var searchText = $('#element_search').val();
        const formData = new FormData();
        formData.append("elementId", elementId);
        formData.append("isDelete", 1);

        fetch("/Element/SaveElement", {
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
                alert('Element deleted sucessfully');
                setTimeout(function () {
                    if (searchText != null && searchText != '') {
                        window.location.href = '/Element/Elements?q=' + searchText;
                    }
                    else {
                        window.location.href = '/Element/Elements';
                    }
                }, 1000);
            })
            .catch(err => {
                alert(err.message);
            });
    }
    return false;

}