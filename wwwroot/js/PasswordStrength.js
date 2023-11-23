const password = document.getElementById("password");
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

    switch (strength) {
        case 0:
            progressBar.style.width = "0%";
            break;
        case 1:
            progressBar.style.width = "25%";
            progressBar.classList.remove("bg-warning");
            progressBar.classList.add("bg-danger");
            break;
        case 2:
            progressBar.style.width = "50%";
            progressBar.classList.remove("bg-danger");
            progressBar.classList.add("bg-warning");
            break;
        case 3:
            progressBar.style.width = "75%";
            progressBar.classList.remove("bg-warning");
            progressBar.classList.add("bg-primary");
            break;
        case 4:
            progressBar.style.width = "100%";
            progressBar.classList.remove("bg-primary");
            progressBar.classList.add("bg-success");
            break;
    }

    strengthValue = strength;
};

password.addEventListener("keyup", () => { CheckPasswordStrength() });

const ReturnStrength = () => {
    return strengthValue;
}