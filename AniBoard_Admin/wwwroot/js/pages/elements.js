const CategoryId_Search = document.getElementById("CategoryId_Search");

CategoryId_Search.addEventListener("change", function () {
    var searchText = $('#element_search').val();
    var catId = $('#CategoryId_Search').val();
    window.location.href = '../Element/Elements?categoryId=' + catId + '&q=' + searchText;
});

function searchelement() {
    var searchText = $('#element_search').val();
    var catId = $('#CategoryId_Search').val();
    //if (searchText != null && searchText != '') {
        window.location.href = '../Element/Elements?categoryId='+ catId +'&q=' + searchText;
    //}
    //else {
    //    window.location.href = '../Element/Elements';
    //}
}

function showaddeditelement_modal(elementId) {
    $("#element_body").empty();

    $.ajax({
        url: "../Element/ShowAddEditElementModal",
        type: "GET",
        data: { "elementId": elementId },
        datatype: "json", 
        success: function (data) {
            $("#element_body").html(data);
        },
    });
}

function gotopage(pageno) {
    //console.log(pageno);
    var searchText = $('#element_search').val();
    var catId = $('#CategoryId').val();
    if (pageno > 0) {

        $.ajax({
            url: "../Element/ShowElementlist",
            type: "GET",
            data: {
                "q": searchText,
                "pageNo": pageno, 
                "catId" : catId
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

$(document).on('keydown', '#userPageInput', function (e) {
    if (e.keyCode === 13) {
        e.preventDefault();
        var pageno = parseInt($(this).val(), 10);
        var totalCount = parseInt($('#hd_totalCount').val(), 10);
        var rowsPerpage = parseInt($('#hd_rowsPerpage').val(), 10);
        var maxPageno = Math.ceil(totalCount / rowsPerpage);

if (pageno > maxPageno) {
    pageno = maxPageno;
        }
        if (pageno > 0) {
            gotopage(pageno);
            $('.pagination li[data-page="' + pageno + '"]').trigger('click');
        }
//console.log(maxPageno);
    }
});
function deleteelement(elementId) {
    if (confirm("Are you sure you want to delete this element?")) {
        var searchText = $('#element_search').val();
        var catId = $('#CategoryId_Search').val();
        const formData = new FormData();
        formData.append("elementId", elementId);
        formData.append("isDelete", 1);

        fetch("../Element/SaveElement", {
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
                    /*if (searchText != null && searchText != '') {*/
                        window.location.href = '../Element/Elements?categoryId=' + catId +'&q=' + searchText;
                    //}
                    //else {
                    //    window.location.href = '../Element/Elements';
                    //}
                }, 1000);
            })
            .catch(err => {
                alert(err.message);
            });
    }
    return false;

}

//pagination work
getPagination();

function getPagination() {
    var totalRows = parseInt($('#hd_totalCount').val());
    var maxRows = parseInt($('#hd_rowsPerpage').val());
    var lastPage = 1;

    // $('#maxRows').
    // on('change', function (evt) {
    //$('.paginationprev').html('');						// reset pagination
    if (maxRows >= totalRows) {
        $('.pagination').hide();
        return false;
    }
    //else {
    //    console.log('pages');
    //}
    lastPage = 1;
    $('.pagination').
        find('li').
        slice(1, -1).
        remove();
    var trnum = 0; // reset tr counter
    //var maxRows = 20;//parseInt($(this).val()); // get Max Rows from select option

    // if (maxRows == 5000) {
    //   $('.pagination').hide();
    // } else {
    $('.pagination').show();
    // }

    //var totalRows = table;//(table + ' tbody tr').length; // numbers of rows
    // $(table + ' tr:gt(0)').each(function () {
    //   // each TR in  table and not the header
    //   trnum++; // Start Counter
    //   if (trnum > maxRows) {
    //     // if tr number gt maxRows

    //     $(this).hide(); // fade it out
    //   }
    //   if (trnum <= maxRows) {
    //     $(this).show();
    //   } // else fade in Important in case if it ..
    // }); //  was fade out to fade it in
    if (totalRows > maxRows) {
        // if tr total rows gt max rows option
        var pagenum = Math.ceil(totalRows / maxRows); // ceil total(rows/maxrows) to get ..
        //	numbers of pages
        for (var i = 1; i <= pagenum;) {
            if (window.CP.shouldStopExecution(0)) break;
            // for each page append pagination li
            $('.pagination #prev').
                before(
                    '<li data-page="' +
                    i +
                    '" onclick="javascript:gotopage(' + i + ');">\
                                      <span>' +
                    i++ +
                    '<span class="sr-only">(current)</span></span>\
                                    </li>').

                show();
        } // end for i
        window.CP.exitedLoop(0);
    } // end if row count > max rows
    $('.pagination [data-page="1"]').addClass('active'); // add active class to the first li
    $('.pagination li').on('click', function (evt) {
        // on click each page
        evt.stopImmediatePropagation();
        evt.preventDefault();
        var pageNum = $(this).attr('data-page'); // get it's number

        //var maxRows = parseInt($('#hd_rowsPerpage').val()); // get Max Rows from select option

        if (pageNum == 'prev') {
            if (lastPage == 1) {
                return;
            }
            pageNum = --lastPage;
        }
        if (pageNum == 'next') {
            if (lastPage == $('.pagination li').length - 2) {
                return;
            }
            pageNum = ++lastPage;
        }

        lastPage = pageNum;
        var trIndex = 0; // reset tr counter
        $('.pagination li').removeClass('active'); // remove active class from all li
        $('.pagination [data-page="' + lastPage + '"]').addClass('active'); // add active class to the clicked
        // $(this).addClass('active');					// add active class to the clicked
        limitPagging();
        // $(table + ' tr:gt(0)').each(function () {
        //   // each tr in table not the header
        //   trIndex++; // tr index counter
        //   // if tr index gt maxRows*pageNum or lt maxRows*pageNum-maxRows fade if out
        //   if (
        //   trIndex > maxRows * pageNum ||
        //   trIndex <= maxRows * pageNum - maxRows)
        //   {
        //     $(this).hide();
        //   } else {
        //     $(this).show();
        //   } //else fade in
        // }); // end of for each tr in table
    }); // end of on click pagination list
    limitPagging();
    // }).
    // val(5).
    // change();

    // end of on select change

    // END OF PAGINATION
}

function limitPagging() {
    // alert($('.pagination li').length)

    if ($('.pagination li').length > 7) {
        if ($('.pagination li.active').attr('data-page') <= 3) {
            $('.pagination li:gt(5)').hide();
            $('.pagination li:lt(5)').show();
            $('.pagination [data-page="next"]').show();
        } if ($('.pagination li.active').attr('data-page') > 3) {
            $('.pagination li:gt(0)').hide();
            $('.pagination [data-page="next"]').show();
            for (let i = parseInt($('.pagination li.active').attr('data-page')) - 2; i <= parseInt($('.pagination li.active').attr('data-page')) + 2; i++) {
                if (window.CP.shouldStopExecution(1)) break;
                $('.pagination [data-page="' + i + '"]').show();

            } window.CP.exitedLoop(1);

        }
    }
}

// $(function () {
//   // Just to append id number for each row
//   $('table tr:eq(0)').prepend('<th> ID </th>');

//   var id = 0;

//   $('table tr:gt(0)').each(function () {
//     id++;
//     $(this).prepend('<td>' + id + '</td>');
//   });
// });
