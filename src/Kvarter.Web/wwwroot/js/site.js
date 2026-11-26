(function () {
    var toggle = document.querySelector('.nav-toggle');
    var nav = document.getElementById('site-nav');
    var desktop = window.matchMedia('(min-width: 861px)');

    if (!toggle || !nav) return;

    function setOpen(open) {
        toggle.setAttribute('aria-expanded', String(open));
        nav.classList.toggle('is-open', open);
    }

    toggle.addEventListener('click', function () {
        if (desktop.matches) return;

        var isOpen = toggle.getAttribute('aria-expanded') === 'true';
        setOpen(!isOpen);
    });

    document.addEventListener('keydown', function (event) {
        if (
            event.key === 'Escape' &&
            !desktop.matches &&
            nav.classList.contains('is-open')
        ) {
            event.preventDefault();
            setOpen(false);
            toggle.focus();
        }
    });

    desktop.addEventListener('change', function () {
        var focusedElement = document.activeElement;

        setOpen(false);

        if (!desktop.matches && nav.contains(focusedElement)) {
            toggle.focus();
        } else if (desktop.matches && focusedElement === toggle) {
            var destination =
                nav.querySelector('a[href]') ||
                document.querySelector('.site-header .brand');

            if (destination) destination.focus();
        }
    });

    setOpen(false);
})();
