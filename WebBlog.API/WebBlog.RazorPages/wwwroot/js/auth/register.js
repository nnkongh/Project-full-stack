import { post } from "../utils/httpClient.js";

document.addEventListener("DOMContentLoaded", function () {
    // lấy id form
    const form = document.getElementById("registerForm");

    // lắng nghe sự kiện submit của form
    form.addEventListener("submit", async function (e) {
        e.preventDefault(); // trình duyệt mặc định sẽ reload trang khi submit form, sử dụng preventDefault để ngăn chặn hành động này

        console.log("Call API register"); 
        const UserName = document.getElementById("UserName").value;
        const Email = document.getElementById("Email").value;
        const Password = document.getElementById("Password").value;
        const ConfirmPassword = document.getElementById("ConfirmPassword").value;

        if (Password !== ConfirmPassword) {
            alert("Mật khẩu không khớp");
            return;
        }
        try {
            const response = await post("https://localhost:7141/api/auth/register", {
                UserName,
                Email,
                Password,
                ConfirmPassword
            });
            if (response.ok) {
                alert("Đăng ký thành công");
                console.log("redirect to login page");
                window.location.href = "https://localhost:7239/auth/login"; // chuyển hướng đến trang đăng nhập sau khi đăng ký thành công
            } else {
                const errText = await response.text();
                alert("Đăng ký thất bại: " + errText);
            }
        } catch (error) {
            alert("Có lỗi xảy ra:" + error.message);
        }
     
    });
});