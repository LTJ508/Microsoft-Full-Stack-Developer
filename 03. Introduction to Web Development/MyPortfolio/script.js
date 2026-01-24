// ============ NAVIGATION ============
function toggleMenu() {
  const nav = document.getElementById("nav-links");
  const hamburger = document.querySelector(".hamburger");
  
  // Toggle the show class
  nav.classList.toggle("show");
  hamburger.classList.toggle("active");
}

// Close menu when clicking outside
document.addEventListener("click", function(event) {
  const nav = document.getElementById("nav-links");
  const hamburger = document.querySelector(".hamburger");
  const navbar = document.querySelector(".navbar");
  
  // Check if click is outside navbar
  if (nav && hamburger && navbar) {
    if (!navbar.contains(event.target) && nav.classList.contains("show")) {
      nav.classList.remove("show");
      hamburger.classList.remove("active");
    }
  }
});

// Smooth scroll with offset
document.querySelectorAll("a[href^='#']").forEach(link => {
  link.addEventListener("click", function(e) {
    e.preventDefault();
    const targetId = this.getAttribute("href");
    const targetElement = document.querySelector(targetId);
    
    if (targetElement) {
      const offsetTop = targetElement.offsetTop - 80;
      window.scrollTo({
        top: offsetTop,
        behavior: "smooth"
      });
      
      // Close mobile menu after click
      const nav = document.getElementById("nav-links");
      const hamburger = document.querySelector(".hamburger");
      if (window.innerWidth <= 768 && nav.classList.contains("show")) {
        nav.classList.remove("show");
        hamburger.classList.remove("active");
      }
    }
  });
});

// ============ TYPING ANIMATION ============
const typingTexts = [
  "Quantum Computing",
  "Quantum Secure Communication",
  "Artificial Intelligence",
  "Machine Learning",
  "Scientific Software Development"
];

let textIndex = 0;
let charIndex = 0;
let isDeleting = false;
let typingSpeed = 100;

function typeEffect() {
  const typingElement = document.getElementById("typing-text");
  if (!typingElement) return;
  
  const currentText = typingTexts[textIndex];
  
  if (isDeleting) {
    typingElement.textContent = currentText.substring(0, charIndex - 1);
    charIndex--;
    typingSpeed = 50;
  } else {
    typingElement.textContent = currentText.substring(0, charIndex + 1);
    charIndex++;
    typingSpeed = 100;
  }
  
  if (!isDeleting && charIndex === currentText.length) {
    isDeleting = true;
    typingSpeed = 2000; // Pause at end
  } else if (isDeleting && charIndex === 0) {
    isDeleting = false;
    textIndex = (textIndex + 1) % typingTexts.length;
    typingSpeed = 500;
  }
  
  setTimeout(typeEffect, typingSpeed);
}

// Start typing effect when page loads
window.addEventListener("load", () => {
  setTimeout(typeEffect, 1000);
});

// ============ SCROLL ANIMATIONS ============
const observerOptions = {
  threshold: 0.1,
  rootMargin: "0px 0px -100px 0px"
};

const observer = new IntersectionObserver((entries) => {
  entries.forEach(entry => {
    if (entry.isIntersecting) {
      entry.target.classList.add("visible");
    }
  });
}, observerOptions);

// Observe all sections and cards
document.addEventListener("DOMContentLoaded", () => {
  document.querySelectorAll("section, .project-card, .cert-card").forEach(el => {
    el.classList.add("fade-in");
    observer.observe(el);
  });
});

// ============ SCROLL TO TOP BUTTON ============
const scrollButton = document.getElementById("scroll-to-top");

window.addEventListener("scroll", () => {
  if (window.pageYOffset > 300) {
    scrollButton.classList.add("visible");
  } else {
    scrollButton.classList.remove("visible");
  }
  
  // Navbar background on scroll
  const header = document.querySelector("header");
  if (window.pageYOffset > 50) {
    header.classList.add("scrolled");
  } else {
    header.classList.remove("scrolled");
  }
});

scrollButton.addEventListener("click", () => {
  window.scrollTo({
    top: 0,
    behavior: "smooth"
  });
});

// ============ PROJECT FILTERS ============
function filterProjects(category) {
  const cards = document.querySelectorAll(".project-card");
  const buttons = document.querySelectorAll("#projects .filter-buttons button");
  
  // Update active button
  buttons.forEach(btn => btn.classList.remove("active"));
  event.target.classList.add("active");
  
  // Animate cards
  cards.forEach((card, index) => {
    if (category === "all" || card.dataset.category === category) {
      setTimeout(() => {
        card.style.display = "block";
        setTimeout(() => card.classList.add("visible"), 10);
      }, index * 50);
    } else {
      card.classList.remove("visible");
      setTimeout(() => card.style.display = "none", 300);
    }
  });
}

// ============ CERTIFICATION FILTERS ============
function filterCertifications(provider) {
  const cards = document.querySelectorAll(".cert-card");
  const buttons = document.querySelectorAll("#certifications .filter-buttons button");
  
  // Update active button
  buttons.forEach(btn => btn.classList.remove("active"));
  event.target.classList.add("active");
  
  // Animate cards
  cards.forEach((card, index) => {
    if (provider === "all" || card.dataset.provider === provider) {
      setTimeout(() => {
        card.style.display = "block";
        setTimeout(() => card.classList.add("visible"), 10);
      }, index * 50);
    } else {
      card.classList.remove("visible");
      setTimeout(() => card.style.display = "none", 300);
    }
  });
}

// Set initial active filter button
document.addEventListener("DOMContentLoaded", () => {
  const projectButton = document.querySelector("#projects .filter-buttons button");
  if (projectButton) projectButton.classList.add("active");
  
  const certButton = document.querySelector("#certifications .filter-buttons button");
  if (certButton) certButton.classList.add("active");
});

// ============ PROJECT CARD INTERACTIONS ============
document.addEventListener("DOMContentLoaded", () => {
  const projectCards = document.querySelectorAll(".project-card");
  
  projectCards.forEach(card => {
    card.addEventListener("mouseenter", function() {
      this.style.transform = "translateY(-10px) scale(1.02)";
    });
    
    card.addEventListener("mouseleave", function() {
      this.style.transform = "translateY(0) scale(1)";
    });
  });
});

// ============ SKILLS ANIMATION ============
document.addEventListener("DOMContentLoaded", () => {
  const skillItems = document.querySelectorAll(".skills-list li");
  
  const skillObserver = new IntersectionObserver((entries) => {
    entries.forEach((entry, index) => {
      if (entry.isIntersecting) {
        setTimeout(() => {
          entry.target.style.opacity = "1";
          entry.target.style.transform = "translateX(0)";
        }, index * 100);
      }
    });
  }, observerOptions);
  
  skillItems.forEach(item => {
    item.style.opacity = "0";
    item.style.transform = "translateX(-20px)";
    item.style.transition = "all 0.5s ease";
    skillObserver.observe(item);
  });
});

// ============ FORM VALIDATION ============
function validateForm() {
  const name = document.getElementById("name").value.trim();
  const email = document.getElementById("email").value.trim();
  const message = document.getElementById("message").value.trim();
  const error = document.getElementById("form-error");
  
  // Clear previous error
  error.textContent = "";
  error.style.display = "none";
  
  // Validation
  if (!name || !email || !message) {
    error.textContent = "All fields are required!";
    error.style.display = "block";
    return false;
  }
  
  // Email validation
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  if (!emailRegex.test(email)) {
    error.textContent = "Please enter a valid email address!";
    error.style.display = "block";
    return false;
  }
  
  // Success animation
  const form = document.getElementById("contactForm");
  form.style.opacity = "0.5";
  
  setTimeout(() => {
    alert("Message sent successfully! I'll get back to you soon.");
    form.reset();
    form.style.opacity = "1";
  }, 500);
  
  return false; // Prevent actual form submission for demo
}

// Real-time form field validation
document.addEventListener("DOMContentLoaded", () => {
  const formInputs = document.querySelectorAll("#contactForm input, #contactForm textarea");
  
  formInputs.forEach(input => {
    input.addEventListener("focus", function() {
      this.style.borderColor = "#2563eb";
      this.style.transform = "scale(1.02)";
    });
    
    input.addEventListener("blur", function() {
      this.style.borderColor = "#ddd";
      this.style.transform = "scale(1)";
      
      // Validate on blur
      if (this.value.trim() === "") {
        this.style.borderColor = "#ff4444";
      }
    });
    
    input.addEventListener("input", function() {
      if (this.value.trim() !== "") {
        this.style.borderColor = "#4CAF50";
      }
    });
  });
});

// ============ INTERACTIVE COUNTER ANIMATION ============
function animateCounter(element, target, duration = 2000) {
  let start = 0;
  const increment = target / (duration / 16);
  
  const timer = setInterval(() => {
    start += increment;
    if (start >= target) {
      element.textContent = target;
      clearInterval(timer);
    } else {
      element.textContent = Math.floor(start);
    }
  }, 16);
}


// ============ CONSOLE EASTER EGG ============
console.log("%c👨‍💻 Hey there! Looking at the code?", "font-size: 20px; color: #2563eb; font-weight: bold;");
console.log("%cI'm Joy Kumar Ghosh - Full Stack Developer & Quantum Computing Researcher", "font-size: 14px; color: #666;");
console.log("%cLet's connect: joydevghosh.joy.jgj@gmail.com", "font-size: 12px; color: #4CAF50;");
