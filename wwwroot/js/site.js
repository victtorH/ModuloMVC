    // Lógica do Dropdown (Agrupamento e Prevenção de Erros)
            const btnNovo = document.getElementById('btn-novo');
            const dropdownNovo = document.getElementById('dropdown-novo');

            btnNovo.addEventListener('click', (e) => {
                e.stopPropagation(); 
                dropdownNovo.classList.toggle('show');
            });

            // Fechar dropdown ao clicar fora
            document.addEventListener('click', (e) => {
                if (!dropdownNovo.contains(e.target) && !btnNovo.contains(e.target)) {
                    dropdownNovo.classList.remove('show');
                }
            });

            // Header Transparente com efeito ao rolar (Scrolled Header)
            const header = document.querySelector('.app-header');
            const mainContent = document.querySelector('.main-content');
            const scrollContainer = mainContent || window;

            const updateHeaderScrolled = () => {
                const scrollTop = mainContent ? mainContent.scrollTop : window.scrollY;
                if (!header) return;

                if (scrollTop > 10) {
                    header.classList.add('scrolled');
                } else {
                    header.classList.remove('scrolled');
                }
            };

            scrollContainer.addEventListener('scroll', updateHeaderScrolled);
            updateHeaderScrolled();

                        // Lógica de Navegação da Sidebar (Estética)
            const navItems = document.querySelectorAll('.nav-list .nav-link');
            navItems.forEach(item => {
                item.addEventListener('click', () => {
                    navItems.forEach(nav => nav.classList.remove('active'));
                    item.classList.add('active');
                });
            });

            // Parallax suave no Blob decorativo
            const blob = document.querySelector('.blob');
            document.addEventListener('mousemove', (e) => {
                const x = (e.clientX / window.innerWidth - 0.5) * 40;
                const y = (e.clientY / window.innerHeight - 0.5) * 40;
                blob.style.transform = `translate(${x}px, ${y}px)`;
            });

            // Efeito Ripple (Ondas globais nos elementos do Main/Header/Footer)
            const addRipple = (e, element) => {
                const circle = document.createElement('span');
                const diameter = Math.max(element.clientWidth, element.clientHeight);
                const radius = diameter / 2;
                const rect = element.getBoundingClientRect();
                
                circle.style.width = circle.style.height = `${diameter}px`;
                circle.style.left = `${e.clientX - rect.left - radius}px`;
                circle.style.top = `${e.clientY - rect.top - radius}px`;
                circle.classList.add('ripple-effect');
                
                Array.from(element.children).forEach(child => {
                    if (child.style) child.style.position = 'relative';
                    if (child.style) child.style.zIndex = '1';
                });

                const existingRipple = element.querySelector('.ripple-effect');
                if (existingRipple) existingRipple.remove();
                
                element.appendChild(circle);
                setTimeout(() => circle.remove(), 600);
            };

            document.querySelectorAll('.nav-link, .btn-main, .btn-secondary, .task-item, .note-card').forEach(btn => {
                btn.style.position = 'relative';
                btn.style.overflow = 'hidden';
                btn.addEventListener('mousedown', (e) => addRipple(e, btn));
            });
        
