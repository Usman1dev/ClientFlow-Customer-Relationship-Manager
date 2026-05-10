// ═══ RUD-CRM JS Interop Helpers ═══
window.rudInterop = {
    showToast: function (message, type) {
        const container = document.getElementById('toast-container');
        if (!container) return;
        const toastId = 'toast-' + Date.now();
        const icons = { success: 'bi-check-circle-fill', error: 'bi-x-circle-fill', warning: 'bi-exclamation-triangle-fill', info: 'bi-info-circle-fill' };
        const colors = { success: '#10b981', error: '#ef4444', warning: '#f59e0b', info: '#0ea5e9' };
        const html = `<div id="${toastId}" class="toast show align-items-center border-0 mb-2" role="alert"
            style="background:var(--rud-surface);border-left:4px solid ${colors[type] || colors.info} !important;box-shadow:var(--rud-shadow-lg);min-width:320px;">
            <div class="d-flex">
                <div class="toast-body d-flex align-items-center gap-2">
                    <i class="bi ${icons[type] || icons.info}" style="color:${colors[type] || colors.info};font-size:1.2rem;"></i>
                    <span style="color:var(--rud-text)">${message}</span>
                </div>
                <button type="button" class="btn-close me-2 m-auto" onclick="this.closest('.toast').remove()"></button>
            </div>
        </div>`;
        container.insertAdjacentHTML('beforeend', html);
        setTimeout(() => { const el = document.getElementById(toastId); if (el) el.remove(); }, 4000);
    },

    toggleSidebar: function () {
        const sidebar = document.querySelector('.sidebar');
        if (sidebar) sidebar.classList.toggle('open');
    },

    closeSidebar: function () {
        const sidebar = document.querySelector('.sidebar');
        if (sidebar) sidebar.classList.remove('open');
    },

    downloadFile: function (url, filename) {
        const a = document.createElement('a');
        a.href = url;
        a.download = filename || 'download';
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
    },

    scrollToTop: function () {
        window.scrollTo({ top: 0, behavior: 'smooth' });
    }
};
