// Enhanced functionality for the lawyer website

document.addEventListener('DOMContentLoaded', function() {
    // Add smooth scrolling to all internal links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth'
                });
            }
        });
    });

    // Add hover effects to competence cards
    const competenceCards = document.querySelectorAll('.competenza-card');
    competenceCards.forEach(card => {
        card.addEventListener('mouseenter', function() {
            this.style.transform = 'translateY(-5px)';
            this.style.transition = 'transform 0.3s ease';
        });
        
        card.addEventListener('mouseleave', function() {
            this.style.transform = 'translateY(0)';
        });
    });

    // NOTE: Contact form handling is now done in contatti.html directly
    // to avoid conflicts with the advanced popup system

    // Add navbar active state management
    const currentPath = window.location.pathname;
    const navLinks = document.querySelectorAll('.navbar-nav .nav-link');
    
    navLinks.forEach(link => {
        link.classList.remove('active');
        const linkPath = new URL(link.href).pathname;
        
        if (currentPath === linkPath || 
            (currentPath === '/' && linkPath === '/') ||
            (currentPath.includes('competenze') && linkPath.includes('competenze')) ||
            (currentPath.includes('contatti') && linkPath.includes('contatti'))) {
            link.classList.add('active');
        }
    });
});