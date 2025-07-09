function searchuser() {
    var searchText = $('#user_search').val();
    if (searchText != null && searchText != '') {
        //alert(searchText);
        window.location.href = '/User/UserList?q=' + searchText;
    }
    else {
        window.location.href = '/User/UserList';
    }
}

function showaddedit_modal(userId) {
    $("#user_body").empty();
    
    $.ajax({
        url: "/User/ShowAddEditModal",
        type: "GET",
        data: { "userId": userId },
        datatype: "json",
        success: function (data) {
            $("#user_body").html(data);
        },
    });
}

function deleteadminuser(userId) {
    if (confirm("Are you sure you want to delete this user?")) {
        var data = {
            CustomerId: userId,
            isDelete: 1
        };
        var formData = new FormData();
        formData.append('data', JSON.stringify(data));
        $.ajax({
            type: "POST",
            url: '/User/SaveUser',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (data) {
                if (data.ok) {
                    alert('User deleted sucessfully');
                    setTimeout(function () {
                        window.location.href = '/User/UserList';
                    }, 1000);
                } else {
                    alert(data.errmsg);
                }
            }
        });
    }


    return false;


}