var contact = {
    init: function () {
        contact.registerEvents();
    },
    registerEvents: function () {
        $('#btnSend').off('click').on('click', function () {
            var email = $('#txtEmail').val();

            $.ajax({
                url: '/Home/RegisterEmail',
                type: 'POST',
                dataType: 'json',
                data: {
                    email: email,
                },
                success: function (res) {
                    if (res.status == true) {
                        alert('Bạn đã đăng ký nhận thông tin sản phẩm mới.');
                        contact.resetForm();
                    }
                }
            });
        });
    },
    resetForm: function () {
        $('#txtEmail').val('');
    }
}
contact.init();