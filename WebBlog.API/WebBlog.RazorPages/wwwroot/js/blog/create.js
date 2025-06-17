import { postForm } from "../utils/httpClient.js";

document.addEventListener("DOMContentLoaded", function () {
    console.log("Create blog page loaded");
    const form = document.getElementById("createForm");
    form.addEventListener("submit", async function (e) {
        e.preventDefault();
        console.log("Form submit event triggered");

        const formData = new FormData(form);
        formData.append("Title", document.getElementById("title").value); // Lấy giá trị tiêu đề từ input
        formData.append("Content", document.getElementById("content").value); // Lấy giá trị nội dung từ input
        formData.append("Tags", document.getElementById("tag").value); // Lấy giá trị tags từ input
        formData.append("Image", document.getElementById("image").files[0]); // Lấy file hình ảnh từ input)



        try {
            const response = await postForm("https://localhost:7141/api/blog/create", formData);
            console.log("Response:", response); // Log the response for debugging
            if (response.ok) {
                alert("Tạo blog thành công");
                window.location.href = "https://localhost:7239/blog/index";
            }
            else {
                const errText = await response.text();
                alert("Tạo blog thất bại: " + errText);
            }
        } catch (error) {
            alert("Tạo blog thất bại: " + error.message);
        }
    });

});