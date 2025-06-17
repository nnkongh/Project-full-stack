import { post } from "../utils/httpClient.js";
document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("forgotForm");
    form.addEventListener("submit", async (e) => {
        console.log("Call API forgot password"); 
        e.preventDefault();
        const Email = document.getElementById("Email").value;
        const clientUri = "https://localhost:7239/auth/resetPassword"

        try {
            console.log("Call API forgot password with email: " + Email);
            const response = await post(`https://localhost:7141/api/auth/forgot`, { Email, clientUri });
            console.log("Response status: " + response.status + " Client uri: " + clientUri);
            if (response.ok) {
                alert("Yêu cầu đặt lại mật khẩu đã được gửi. Vui lòng kiểm tra email của bạn.");
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