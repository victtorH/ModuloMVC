        // Animações de Entrada
        window.addEventListener('load', () => {
            gsap.to(".animate-in", {
                opacity: 1,
                y: 0,
                duration: 0.8,
                stagger: 0.1,
                ease: "power3.out"
            });
            
            // Animação da arte de fundo
            gsap.to(".bloom-shape", {
                x: 20,
                y: 20,
                duration: 8,
                repeat: -1,
                yoyo: true,
                ease: "sine.inOut"
            });
        });

        // Lógica da Barra de Progresso no Topo
        const inputs = document.querySelectorAll('.input-control');
        const progressBar = document.getElementById('progressBar');

        function updateProgress() {
            const totalFields = inputs.length;
            let completedFields = 0;

            inputs.forEach(input => {
                if (input.value.trim().length > 0 && input.checkValidity()) {
                    completedFields++;
                    input.style.borderColor = "var(--accent-color)";
                } else if (input.value.trim().length > 0) {
                    input.style.borderColor = "#f43f5e";
                } else {
                    input.style.borderColor = "#e2e8f0";
                }
            });

            const progress = (completedFields / totalFields) * 100;
            gsap.to(progressBar, {
                width: progress + "%",
                duration: 0.4,
                ease: "power1.out"
            });
        }

        inputs.forEach(input => {
            input.addEventListener('input', updateProgress);
        });

        // Validação
      
            if (!hasError) {
                const btn = document.getElementById('submitBtn');
                btn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span> Creating...';
                btn.disabled = true;

                setTimeout(() => {
                btn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>  conect account';
                btn.disabled = false;
                  
                }, 6000);
            }
