import { get } from "../utils/httpClient.js";


document.addEventListener("DOMContentLoaded", async function () {
    const container = document.getElementById("blog-detail");
    const urlParams = new URLSearchParams(window.location.search); // url params dùng để lấy tham số từ URL 
    const id = urlParams.get("id"); // lấy giá trị của tham số id từ URL"

    //Kiểm tra xem id có tồn tại không
    if (!id) {
        alert("Không tìm thấy id bài viết");
        window.location.href = "/Blog/Index";s
        return;
    }
    try {
        // Lấy thông tin từ response
        const response = await get(`https://localhost:7141/api/blog/get/${id}`); 
        if (!response.ok) {
            throw new Error("Không tìm thấy bài viết");
        }
        // chuyển đổi thành kiểu json
        const blog = await response.json();
        container.innerHTML = `
            <dl class="row">
                <dt class="col-sm-2">Tiêu đề</dt>
                <dd class="col-sm-10">${blog.Title}</dd>

                <dt class="col-sm-2">Nội dung</dt>
                <dd class="col-sm-10">${blog.Content}</dd>"

                <dt class="col-sm-2">Tags</dt>
                <dd class="col-sm-10">${blog.Tags}</dd>

                <dt class="col-sm-2">Ngày đăng</dt>
                <dd class="col-sm-10">${new Date(blog.CreatedAt).toLocaleDateString("vi-VN")}</dd>

                <dt class="col-sm-2">Hình ảnh</dt>
                <dd class="col-sm-10"> ${blog.Image ? `<img src="{blog.Image}" alt="Image post" style="max-height: 200px"/>` : ""};
            </dl>
            `
    } catch (error) {
        container.innerHTML = `<p class="text-danger">Lỗi: ${error.message}</p>`;
    }
});