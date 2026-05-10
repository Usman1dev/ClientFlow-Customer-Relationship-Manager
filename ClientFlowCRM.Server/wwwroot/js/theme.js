// ═══ RUD-CRM Theme Manager ═══
window.themeManager = {
    init: function () {
        const saved = localStorage.getItem('rud-theme') || 'light';
        document.documentElement.setAttribute('data-theme', saved);
    },
    toggle: function () {
        const current = document.documentElement.getAttribute('data-theme') || 'light';
        const next = current === 'dark' ? 'light' : 'dark';
        document.documentElement.setAttribute('data-theme', next);
        localStorage.setItem('rud-theme', next);
        return next;
    },
    get: function () {
        return document.documentElement.getAttribute('data-theme') || 'light';
    }
};

// Auto-initialize theme
document.addEventListener('DOMContentLoaded', function () {
    window.themeManager.init();
});

// Run immediately as well for prerendered pages
window.themeManager.init();
