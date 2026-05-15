     document.addEventListener('DOMContentLoaded', () => {


            // Lógica do Dropdown (Restaurada para Vanilla JS)
            const btnNovo = document.getElementById('btn-novo');
            const dropdownNovo = document.getElementById('dropdown-novo');

            btnNovo.addEventListener('click', (e) => {
                e.stopPropagation(); 
                dropdownNovo.classList.toggle('show');
            });

            document.addEventListener('click', (e) => {
                if (!dropdownNovo.contains(e.target) && !btnNovo.contains(e.target)) {
                    dropdownNovo.classList.remove('show');
                }
            });

            // Header Transparente com efeito ao rolar (Scroll)
            const header = document.querySelector('.app-header');
            const mainContent = document.querySelector('.main-content');

            mainContent.addEventListener('scroll', () => {
                if (mainContent.scrollTop > 10) {
                    header.classList.add('scrolled');
                } else {
                    header.classList.remove('scrolled');
                }
            });

            // Visibilidade do Status nas Tarefas
            const tasks = document.querySelectorAll('.task-item');
            tasks.forEach(task => {
                task.addEventListener('click', () => {
                    task.classList.toggle('done');
                });
            });


            // Lógica para alternar o Empty State
            const btnToggleEmpty = document.getElementById('toggle-empty-btn');
            const notesContent = document.getElementById('notes-content');
            const emptyStateContent = document.getElementById('empty-state-content');
            const linkVerTodas = document.getElementById('ver-todas-link');
            let isEmpty = false;

            btnToggleEmpty.addEventListener('click', () => {
                isEmpty = !isEmpty;
                if (isEmpty) {
                    notesContent.classList.replace('d-flex', 'd-none');
                    notesContent.style.display = 'none';
                    emptyStateContent.classList.add('active');
                    btnToggleEmpty.textContent = 'Mostrar Dados';
                    linkVerTodas.classList.add('d-none');
                } else {
                    notesContent.classList.replace('d-none', 'd-flex');
                    notesContent.style.display = '';
                    emptyStateContent.classList.remove('active');
                    btnToggleEmpty.textContent = 'Testar Empty State';
                    linkVerTodas.classList.remove('d-none');
                }
            });

            // Parallax suave no Blob decorativo
            const blob = document.querySelector('.blob');
            document.addEventListener('mousemove', (e) => {
                const x = (e.clientX / window.innerWidth - 0.5) * 40;
                const y = (e.clientY / window.innerHeight - 0.5) * 40;
                blob.style.transform = `translate(${x}px, ${y}px)`;
            });

            // Efeito Ripple (Ondas) em botões e cards
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

            document.querySelectorAll('.nav-item-custom, .btn-custom-primary, .btn-light, .task-item, .note-card, .dropdown-item-custom').forEach(btn => {
                btn.style.position = 'relative';
                btn.style.overflow = 'hidden';
                btn.addEventListener('mousedown', (e) => addRipple(e, btn));
            });
        });