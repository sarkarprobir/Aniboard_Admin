const fileInput = document.getElementById("fileInput");
const previewContainer = document.getElementById("previewContainer");
const previewImage = document.getElementById("previewImage");
const uploadStatus = document.getElementById("uploadStatus");

// 📌 Show local preview instantly
fileInput.addEventListener("change", function () {
    const file = fileInput.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            previewContainer.style.display = "block";
            previewImage.src = e.target.result; // Local preview
            uploadStatus.textContent = "Not uploaded yet.";
        };
        reader.readAsDataURL(file);
    } else {
        previewContainer.style.display = "none";
    }
});

// 📌 Handle upload
document.getElementById("uploadForm").addEventListener("submit", function (e) {
    e.preventDefault();

    const formData = new FormData();
    formData.append("file", fileInput.files[0]);
    formData.append("imageId", document.getElementById("imageId").value);
    formData.append("customName", document.getElementById("imageName").value);

    uploadStatus.textContent = "Uploading...";

    fetch("/Element/SaveBackgroundImage", {
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
            alert('Background image created sucessfully');
            setTimeout(function () {
                window.location.href = '/Element/BackgroundImage';
            }, 1000);
        })
        .catch(err => {
            alert(err.message);
            //uploadStatus.textContent = "Error: " + err.message;
        });
});