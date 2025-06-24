import { post } from "../utils/httpClient.js";


document.addEventListener("DOMContentLoaded", function () {

    const form = document.getElementById("loginForm");

    if (!form) return;

    form.addEventListener("submit", async function (e) {
        e.preventDefault();
        const userName = document.getElementById("userName").value;
        const passWord = document.getElementById("password").value;

        try {
            const response = await post("https://localhost:7141/api/auth/login", { 
                userName,
                passWord
            });


            if (response.ok) {
                console.log("redirect to blog page");
                window.location.href = "https://localhost:7239/blog/index";
            } else {
                const errText = await response.text();
                alert("Đăng nhập thất bại: " + errText);
            }
        } catch (error) {
            alert("Đăng nhập thất bại: " + error.message);
        }
    });
});