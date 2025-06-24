// auth.js hoặc main.js

async function logout() {
    try {
        await fetch(`https://localhost:7141/api/auth/logout`, {
            method: "POST",
            credentials: "include"
        });
    } catch (error) {
        console.error("Lỗi khi logout:", error);
    } finally {
        alert("Đăng xuất thành công");
        window.location.href = "/Auth/Login"; // luôn redirect
    }
}

document.addEventListener("DOMContentLoaded", function () {
    const btn = document.getElementById("logoutBtn");
    if (btn) {
        btn.addEventListener("click", logout);
    }
});
