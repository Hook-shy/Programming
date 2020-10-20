var cropper;

$('#pic-input').change(function (ev) {
    console.log(ev);
    const file = this.files[0];
    if (!file) return;
    if (cropper) cropper.destroy();
    $('#pic-cropper').prop('src', URL.createObjectURL(file));
    cropper = new Cropper(document.getElementById('pic-cropper'), {
        viewMode: 1,
        aspectRatio: 1,
        autoCropArea: 1,
        dragMode: 'move',
        minContainerWidth: 600,
        minContainerHeight: 364,
        ready: (e) => {
            $('.img-preview').prop('src', cropper.getCroppedCanvas().toDataURL('image/jpeg', 1.0));
        },
        cropend: (e) => {
            $('.img-preview').prop('src', cropper.getCroppedCanvas().toDataURL('image/jpeg', 1.0));
        }
    });
    $('#avatar-modal').modal('show');
});

$('#left90').click(() => {
    if (!cropper) return;
    cropper.rotate(-90);
    $('.img-preview').prop('src', cropper.getCroppedCanvas().toDataURL('image/jpeg', 1.0));
});
$('#right90').click(() => {
    if (!cropper) return;
    cropper.rotate(90);
    $('.img-preview').prop('src', cropper.getCroppedCanvas().toDataURL('image/jpeg', 1.0));
});
$('#img-submit').click(() => {
    if (!cropper) return;
    var formData = new FormData();
    formData.append("file", encodeURIComponent(cropper.getCroppedCanvas().toDataURL('image/jpeg', 1.0)));
    formData.append("fileName", "photo.jpg");
    $.ajax({
        url: "/Me/UpdateImg",
        type: 'POST',
        data: formData,
        timeout: 10000,
        dataType: 'multipart/form-data',
        async: true,
        cache: false,
        contentType: false,
        processData: false,
        success: function (result) { },
        error: function (returndata) { }
    });
    $('#avatar-modal').modal('hide');
});