import { Component, OnInit } from '@angular/core';
import { loadFull } from 'tsparticles';
import { ClickMode, Container, Engine, HoverMode, MoveDirection, OutMode } from 'tsparticles-engine';
import { TelegramLoginService } from './share/telegram-login.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  title = 'TgDrive';
  loggedIn: boolean = false;

  constructor(private loginService: TelegramLoginService) {
  }

  ngOnInit(): void {
    this.loginService.loggedIn$.subscribe(isLoggedIn => this.loggedIn = isLoggedIn);
  }

  particlesOptions = {
    background: {
      color: {
        value: 'transparent',
      },
    },
    fpsLimit: 30,
    interactivity: {
      events: {
        onHover: {
          enable: true,
          mode: HoverMode.repulse,
        },
        resize: true,
      },
      modes: {
        repulse: {
          distance: 200,
          duration: 0.4,
        },
      },
    },
    particles: {
      color: {
        value: '#222222',
      },
      links: {
        color: '#222222',
        distance: 250,
        enable: true,
        opacity: 0.5,
        width: 1,
      },
      collisions: {
        enable: false,
      },
      move: {
        direction: MoveDirection.none,
        enable: true,
        outModes: {
          default: OutMode.bounce,
        },
        bounce: false,
        random: false,
        speed: 1,
        straight: false,
      },
      number: {
        density: {
          enable: true,
          area: 800,
        },
        value: 30,
      },
      opacity: {
        value: 0.8,
      },
      rotate: {
        value: {
          min: 0,
          max: 360
        },
        direction: "random",
        animation: {
          enable: true,
          speed: 5
        }
      },
      shape: {
        type: 'image',
        options: {
          "image": {
            "src": "assets/tg-logo.svg",
            "width": 200,
            "height": 200,
            "replaceColor": true
          }
        }
      },
      size: {
        value: { min: 1, max: 20 },
      },
    },
    detectRetina: true,
  };

  particlesLoaded(container: Container): void {
    console.log(container);
  }

  async particlesInit(engine: Engine): Promise<void> {
    await loadFull(engine);
  }
}
