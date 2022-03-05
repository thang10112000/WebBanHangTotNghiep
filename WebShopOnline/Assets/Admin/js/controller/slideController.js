﻿// gọi ajax kích hoạt , khóa status
var slide = {
    init: function () {
        slide.registerEvents();
    },
    registerEvents: function () {
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this)
            var id = btn.data('id');
            $.ajax({
                url: "/Admin/Slide/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",

                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        btn.text('Kích hoạt');
                    } else {
                        btn.text('Khóa');
                    }
                }
            });
        });
    }
}
slide.init();