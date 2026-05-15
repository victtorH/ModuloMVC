document.addEventListener("DOMContentLoaded", () => {
    
    // Registrar o ScrollTrigger do GSAP
    gsap.registerPlugin(ScrollTrigger);

    console.log("🚀 Animações Carregadas!");

    // 1. Entrada do Background 404 (Efeito de Profundidade)
    gsap.from(".bg-text-404", {
        duration: 2.5,
        opacity: 0,
        scale: 0.9,
        ease: "power4.out"
    });

    // 2. Animação de Levitação Infinita no 404 (Gestalt: Destino Comum)
    gsap.to(".bg-text-404", {
        y: 20, // Menor amplitude de levitação para ser mais sutil
        duration: 5, // Mais devagar para ser mais sereno
        repeat: -1,
        yoyo: true,
        ease: "sine.inOut"
    });

    // 3. Revelação do Conteúdo Central (Heurística #1)
    const tl = gsap.timeline();
    tl.from(".content-wrapper h2", { y: 30, opacity: 0, duration: 1, delay: 0.8, ease: "power3.out" })
      .from(".content-wrapper p", { y: 20, opacity: 0, duration: 0.8 }, "-=0.6")
      .from(".btn-recovery", { scale: 0.9, opacity: 0, duration: 0.7, ease: "back.out(1.7)" }, "-=0.4");

    // 4. Newsletter Slide Up (Heurística de Usabilidade #10)
    gsap.from(".js-newsletter-animate", {
        scrollTrigger: {
            trigger: ".js-newsletter-animate",
            start: "top 80%", // Começa quando a newsletter estiver 80% visível na tela
            toggleActions: "play none none none"
        },
        y: 100,
        opacity: 0,
        duration: 1.2,
        ease: "power3.out"
    });
    tl.to(".btn-recovery", { 
      opacity: 1, 
      scale: 1, 
      duration: 0.5, 
      visibility: "visible", // Força a visibilidade
      ease: "back.out(1.7)" 
  }, "-=0.3");

    // 5. Interação no Botão Subscribe (Microinteração / Lei de Fitts)
    const btnSub = document.querySelector(".btn-subscribe");
    if (btnSub) {
        btnSub.addEventListener("mouseenter", () => {
            gsap.to(btnSub, { rotation: 2, duration: 0.1, repeat: 3, yoyo: true });
        });
        btnSub.addEventListener("mouseleave", () => {
            gsap.to(btnSub, { rotation: 0, duration: 0.2 });
        });
    }
});