const password = document.getElementById("password");
const passwordStrengthText = document.getElementById("password-strength-text");
const progressContainer = document.getElementById("progress");
const progressBar = document.getElementById("progress-bar");
let strengthValue = 0;

const CheckPasswordStrength = () => {
    var strength = 0;

    if (password.value === 0) {
        progressContainer.classList.add("d-none");
    } else {
        progressContainer.classList.remove("d-none");
    }

    if (password.value.match(/[a-z]+/)) {
        strength += 1;
    }
    if (password.value.match(/[A-Z]+/)) {
        strength += 1;
    }
    if (password.value.match(/[0-9]+/)) {
        strength += 1;
    }
    if (password.value.match(/[$@#&!]+/)) {
        strength += 1;
    }
    if (password.value.length > 5) {
        strength += 1;
    }

    switch (strength) {
        case 0:
            progressBar.style.width = "0%";
            passwordStrengthText.innerText = "";
            progressContainer.classList.add("d-none");
            break;
        case 1:
            progressBar.style.width = "20%";
            progressBar.classList.remove("bg-danger");
            progressBar.classList.remove("bg-warning");
            progressBar.classList.add("bg-danger");
            passwordStrengthText.innerText = "Weak";
            break;
        case 2:
            progressBar.style.width = "40%";
            progressBar.classList.remove("bg-danger");
            progressBar.classList.remove("bg-warning");
            progressBar.classList.add("bg-danger");
            passwordStrengthText.innerText = "Weak";
            break;
        case 3:
            progressBar.style.width = "60%";
            progressBar.classList.remove("bg-danger");
            progressBar.classList.remove("bg-warning");
            progressBar.classList.add("bg-warning");
            passwordStrengthText.innerText = "Medium";
            break;
        case 4:
            progressBar.style.width = "80%";
            progressBar.classList.remove("bg-danger");
            progressBar.classList.remove("bg-warning");
            progressBar.classList.add("bg-warning");
            passwordStrengthText.innerText = "Medium";
            break;
        case 5:
            progressBar.style.width = "100%";
            progressBar.classList.remove("bg-danger");
            progressBar.classList.remove("bg-warning");
            progressBar.classList.add("bg-success");
            passwordStrengthText.innerText = "Strong";
            break;
    }

    strengthValue = strength;
};

password.addEventListener("keyup", () => { CheckPasswordStrength() });

const ReturnStrength = () => {
    return strengthValue;
}