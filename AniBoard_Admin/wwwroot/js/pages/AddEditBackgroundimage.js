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
    var imgId = document.getElementById("imageId").value;
    var searchText = $('#backgroundimage_search').val();
    const formData = new FormData();
    formData.append("file", fileInput.files[0]);
    formData.append("imageId", document.getElementById("imageId").value);
    formData.append("customName", document.getElementById("imageName").value);

    uploadStatus.textContent = "Uploading...";
    console.log(formData);
    fetch("../Element/SaveBackgroundImage", {
        method: "POST",
        body: formData
    })
        .then(response => {
            if (!response.ok) {
                alert(response.Errors);
            } 
            return response.json();
        })
        .then(data => {
            if (imgId > 0) {
                alert('Background image updated sucessfully');
            }
            else {
                alert('Background image created sucessfully');
            }
            
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
            alert(err.Errors);
            //uploadStatus.textContent = "Error: " + err.message;
        });
});