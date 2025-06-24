import { blogIndex, get, remove } from "../utils/httpClient.js";
import { renderPosts } from "./renderPosts.js";
import { renderPagination } from "../pagination/pagination.js";

let currentPage = 1;
const pageSize = 5;
document.addEventListener("DOMContentLoaded", async function () {
    console.log("Blog index page loaded");
    loadBlogs(currentPage);
});
async function loadBlogs(page) {
    const container = document.getElementById("post-container");

    try {
        const res = await blogIndex("https://localhost:7141/api/blog/blogs", page, pageSize);///
        if (!res.ok) {
            throw new Error("Không thể tải bài viết");
        }

        const resposts = await res.json();
        const posts = resposts.items || [];
        const pageIndex = resposts.index;
        const totalPages = resposts.totalPages;


        renderPosts(posts);
        renderPagination(pageIndex, totalPages, loadBlogs);


    } catch (error) {
        container.innerHTML = `<p class="text-danger">Lỗi: ${error.message}</p>`;
    }
}
