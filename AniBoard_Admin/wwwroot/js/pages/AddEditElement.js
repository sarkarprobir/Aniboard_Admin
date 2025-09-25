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
    var imgId = document.getElementById("elementId").value;
    var searchText = $('#element_search').val();
    var catId = $('#CategoryId').val();
    const formData = new FormData();
    formData.append("file", fileInput.files[0]);
    formData.append("elementId", imgId);
    formData.append("elementName", document.getElementById("elementName").value);
    formData.append("categoryId", document.getElementById("CategoryId").value);
    formData.append("imageTag", document.getElementById("imageTag").value);
    uploadStatus.textContent = "Uploading...";

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
            console.log(data);
            if (data.ok == false) {
                alert(data.errors);
            }
            else {
                if (imgId > 0) {
                    alert('Element updated sucessfully');
                }
                else {
                    alert('Element created sucessfully');
                }
            }


            setTimeout(function () {
                //if (searchText != null && searchText != '') {
                    window.location.href = '../Element/Elements?categoryId=' + catId +'&q=' + searchText;
                //}
                //else {
                //    window.location.href = '../Element/Elements';
                //}
                 
            }, 1000);
        })
        .catch(err => {
            alert(err.message);
            //uploadStatus.textContent = "Error: " + err.message;
        });
});

