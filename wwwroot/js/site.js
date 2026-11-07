// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



function toggleSingleField(formId, fieldId, buttonId, inputId, editText, saveText) {
    const form = document.getElementById(formId);
    const field = document.getElementById(fieldId);
    const button = document.getElementById(buttonId);
    const input = document.getElementById(inputId);

    if (!form || !field || !button || !input) return;

    if (field.hidden) {
        field.hidden = false;
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
        if (field) field.hidden = false;

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

document.addEventListener("DOMContentLoaded", restoreProfileForm);
