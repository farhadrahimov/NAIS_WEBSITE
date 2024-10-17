// Open the modal
function openModal(imagePath, name) {
    var modal = document.getElementById("modal");
    var modalImg = document.getElementById("modal-img");
    var captionText = document.getElementById("caption");

    modal.style.display = "block";
    modalImg.src = imagePath;
    captionText.innerHTML = name;
}

function openModalWithContent(element) {
    var imgSrc = element.getAttribute('data-img');
    var title = element.getAttribute('data-title');
    var content = element.getAttribute('data-content');

    document.getElementById("modal-img").src = imgSrc;
    document.getElementById("modal-title").innerText = title;
    document.getElementById("modal-text").innerText = content.split('/').pop();
    document.getElementById("modal").style.display = "block";
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