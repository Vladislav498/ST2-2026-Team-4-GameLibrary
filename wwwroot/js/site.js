
function toggleSingleField(formId, fieldId, buttonId, inputId, editText, saveText, layoutdiv) {
    const form = document.getElementById(formId);
    const field = document.getElementById(fieldId);
    const button = document.getElementById(buttonId);
    const input = document.getElementById(inputId);
    const layout = document.getElementById(layoutdiv);

    if (!form || !field || !button || !input) return;

    if (field.hidden) {
        field.hidden = false;
        input.disabled = false;
        layout.style.display = "block";
        form.style.marginTop = "30px";
        button.textContent = saveText;
    } else {
        if (!form.reportValidity()) return;

        form.requestSubmit();
    }
}

function toggleMultipleFields(formId, fieldClass, buttonId, inputClass, editText, saveText, layoutdiv) {
    const form = document.getElementById(formId);
    const button = document.getElementById(buttonId);

    if (!form || !button) return;

    const fields = form.querySelectorAll("." + fieldClass);
    const inputs = form.querySelectorAll("." + inputClass);
    const layout = document.getElementById(layoutdiv);

    if (!fields.length) return;

    if (fields[0].hidden) {
        fields.forEach(field => {
            field.hidden = false;
        });

        inputs.forEach(input => {
            input.disabled = false;
        });

        button.textContent = saveText;
        layout.style.display = "block";
        form.style.marginTop = "30px";
        form.style.marginBottom = "30px";

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
    }

    if (formId === "emailForm") {
        form.querySelectorAll(".changeEmail").forEach(field => {
            field.hidden = false;
            field.disabled = false;
        });

        if (button) button.textContent = "Save Email";
    }

    if (formId === "passwordForm") {
        form.querySelectorAll(".changePassword").forEach(field => {
            field.hidden = false;
            field.disabled = false;
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
