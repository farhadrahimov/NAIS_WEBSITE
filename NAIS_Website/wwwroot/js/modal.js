// Open the modal
function openModal(imagePath, name) {
    var modal = document.getElementById("modal");
    var modalImg = document.getElementById("modal-img");
    var captionText = document.getElementById("caption");

    modal.style.display = "block";
    modalImg.src = imagePath;
    captionText.innerHTML = name;
}

// Close the modal
function closeModal() {
    var modal = document.getElementById("modal");
    modal.style.display = "none";
}

// Close modal when clicking outside the image
window.onclick = function (event) {
    var modal = document.getElementById("modal");
    if (event.target === modal) {
        modal.style.display = "none";
    }
}