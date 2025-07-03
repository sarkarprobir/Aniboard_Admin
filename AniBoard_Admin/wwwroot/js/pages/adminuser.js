

function searchuser() {
    var searchText = $('#admin_user_search').val();
    if (searchText != null && searchText != '') {
        alert(searchText);
    }
}

function showaddedit_modal(userId) {
    $("#adminuser_body").empty();
    //var divspinner = '<div class="spinner-border text-success" role="status"><span class="visually-hidden"></span></div>';
    //$("#edit_account_modal").val(divspinner);
    //$('#edit_log_partial_caption').text(id);
    $.ajax({
        url: "/AdminUser/ShowAddEditModal",
        type: "GET",
        data: { "userId": userId },
        datatype: "json",
        success: function (data) {
            $("#adminuser_body").html(data);
            //$("#AdminUser-modal").modal('show');
        },
    });
}

function deleteadminuser(userId) {
    var data = {
        adminId: userId,
        isDelete: 1
    };
    var formData = new FormData();
    formData.append('data', JSON.stringify(data));
    $.ajax({
        type: "POST",
        url: '/AdminUser/SaveAdminUser',
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (data) {
            if (data.ok) {
                alert('User deleted sucessfully');
                setTimeout(function () {
                    window.location.href = '/AdminUser/AdminUserList';
                }, 1000);
            } else {
                alert(data.errmsg);
            }
        }
    });

    return false;


}