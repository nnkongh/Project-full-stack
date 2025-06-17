import { post } from "../utils/httpClient.js";


document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("resetForm");
    const urlParams = new URLSearchParams(window.location.search);
    const token = urlParams.get("token");
    const email = urlParams.get("email");
    const userName = document.getElementById("userName");
    
    form.addEventListener("submit", async (e) => {
        console.log("Form submit event triggered for reset password");
        e.preventDefault();

        const Password = document.getElementById("password").value;
        const ConfirmPassword = document.getElementById("ConfirmPassword").value;

        if (Password !== ConfirmPassword) {
            alert("Mật khẩu và xác nhận mật khẩu không khớp. Vui lòng kiểm tra lại.");
        }

        if (!token || !email) {
            alert("Yêu cầu không hợp lệ. Vui lòng kiểm tra lại liên kết đặt lại mật khẩu.");
            return;
        }

        const body = { email, userName, token, Password, ConfirmPassword };
        console.log("Call API reset password with body: ", body); 
        try {
            const response = await post(`https://localhost:7141/api/auth/reset`, body);
            if (response.ok) {
                alert("Yêu cầu đặt lại mật khẩu đã được gửi. Vui lòng kiểm tra email của bạn.");
                window.location.href = "https://localhost:7239/auth/login"; // Redirect to login page after successful reset
            }
            else {
                const errText = await response.text();
                alert("Có lỗi xảy ra: " + errText);
            }

        } catch (error) {
            alert("Có lỗi xảy ra: " + error.message);
        }
    })
});