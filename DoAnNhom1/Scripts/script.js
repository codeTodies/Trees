function ChangeImage(UploadImage, previewImg) {
    if (UploadImage.files && UploadImage.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewImg).attr('src', e.target.result);
        };
        reader.readAsDataURL(UploadImage.files[0]); // Đọc tập tin hình ảnh và chuyển đổi thành dạng dữ liệu URL
    }
}