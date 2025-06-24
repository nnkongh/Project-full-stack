import { get, postForm} from "../utils/httpClient.js";


document.addEventListener("DOMContentLoaded", async function () {
    const urlParams = new URLSearchParams(window.location.search); // url params dùng để lấy tham số từ URL 
    console.log(urlParams);
    const id = urlParams.get("id"); // lấy giá trị của tham số id từ URL"
    console.log(id);
    if (!id) {
        alert("Không tìm thấy id bài viết");
        window.location.href = "/Blog/Index";
        return;
    }

    // Lấy thông tin bài viết từ API
    const res = await get(`https://localhost:7141/api/blog/${id}`);

    if (!res.ok) {
        alert("Không tìm thấy bài viết");
        window.location.href = "/Blog/Index";
        return;
    }
    //Lấy dữ liệu bài viết từ resposne
    const post = await res.json();

    document.getElementById("id").value = post.id;
    document.getElementById("title").value = post.title;
    document.getElementById("content").value = post.content;
    document.getElementById("tags").value = post.tags;
    document.getElementById("imagePreview").src = post.image || "";

    
    // Xử lí cập nhật
    document.getElementById("editPost").addEventListener("submit", async e => {
        e.preventDefault();

        const form = document.getElementById("editPost");
        const formData = new FormData();
        formData.append("id", document.getElementById("id").value);
        formData.append("title", document.getElementById("title").value);
        formData.append("content", document.getElementById("content").value);
        formData.append("tags", document.getElementById("tags").value);
        formData.append("createdAt", post.createdAt);
        console.log();
        const fileInput = document.getElementById("image");
        if (fileInput.files.length > 0) {
            formData.append("Image", fileInput.files[0]);
        }

        const res = await postForm(`https://localhost:7141/api/blog/update/${id}`, formData);
        console.log(formData);
        if (res.ok) {
            alert("Cập nhật bài viết thành công");
            window.location.href = "/Blog/Index"; // chuyển hướng về trang danh sách bài viết
        } else {
            const errText = await res.text();
            alert("Cập nhật thất bại: " + errText);
        }
    });
});