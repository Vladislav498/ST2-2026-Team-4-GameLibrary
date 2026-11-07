
function toggleSingleField(formId, fieldId, buttonId, inputId, editText, saveText) {
    const form = document.getElementById(formId);
    const field = document.getElementById(fieldId);
    const button = document.getElementById(buttonId);
    const input = document.getElementById(inputId);

    if (!form || !field || !button || !input) return;

    if (field.hidden) {
        field.hidden = false;
        input.disabled = false;
        button.textContent = saveText;
    } else {
        if (!form.reportValidity()) return;
        form.requestSubmit();
    }
}

function toggleMultipleFields(formId, fieldClass, buttonId, inputClass, editText, saveText) {
    const form = document.getElementById(formId);
    const button = document.getElementById(buttonId);

    if (!form || !button) return;

    const fields = form.querySelectorAll("." + fieldClass);
    const inputs = form.querySelectorAll("." + inputClass);

    if (!fields.length) return;

    const shouldShow = Array.from(fields).some(field => field.hidden);

    if (shouldShow) {
        fields.forEach(field => {
            field.hidden = false;
        });

        inputs.forEach(input => {
            input.disabled = false;
        });

        button.textContent = saveText;
    } else {
        if (!form.reportValidity()) return;
        form.requestSubmit();
    }
}

function restoreProfileForm() {
    const container = document.querySelector("[data-open-profile-form]");
    const formId = container?.dataset.openProfileForm;

    if (!formId) return;

    const form = document.getElementById(formId);
    if (!form) return;

    const button = form.querySelector("button");

    if (formId === "nameForm") {
        const field = document.getElementById("nameField");
        const input = document.getElementById("nameInput");

        if (field) field.hidden = false;
        if (input) input.disabled = false;

        if (button) button.textContent = "Save Name";
    } else if (formId === "emailForm") {
        form.querySelectorAll(".changeEmail").forEach(field => {
            field.hidden = false;
        });

        form.querySelectorAll(".emailInput").forEach(input => {
            input.disabled = false;
        });

        if (button) button.textContent = "Save Email";
    } else if (formId === "passwordForm") {
        form.querySelectorAll(".changePassword").forEach(field => {
            field.hidden = false;
        });

        form.querySelectorAll(".passwordInput").forEach(input => {
            input.disabled = false;
        });

        if (button) button.textContent = "Save Password";
    }
}

document.addEventListener("DOMContentLoaded", function () {

 
    restoreProfileForm();

 
    const toggle = document.getElementById("navbarToggle");
    const menu = document.getElementById("navbarMenu");

    if (toggle && menu) {
        toggle.addEventListener("click", function () {
            menu.classList.toggle("show");

            const isExpanded = menu.classList.contains("show");
            toggle.setAttribute("aria-expanded", isExpanded);
        });
    }

});
