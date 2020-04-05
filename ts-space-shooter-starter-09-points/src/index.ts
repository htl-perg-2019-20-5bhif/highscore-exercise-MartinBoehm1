import { Scene, Types, CANVAS, Game, Physics, Input, GameObjects } from 'phaser';
import { Spaceship, Direction } from './spaceship';
import { Bullet } from './bullet';
import { Meteor } from './meteor';
import { Player } from './player';
const axios = require('axios').default;

/**
 * Space shooter scene
 * 
 * Learn more about Phaser scenes at 
 * https://photonstorm.github.io/phaser3-docs/Phaser.Scenes.Systems.html.
 */
class ShooterScene extends Scene {
    private spaceShip: Spaceship;
    private meteors: Physics.Arcade.Group;
    private bullets: Physics.Arcade.Group;
    private points: GameObjects.Text;
    private gameoverText: GameObjects.Text;
    private bulletTime = 0;
    private meteorTime = 0;

    private cursors: Types.Input.Keyboard.CursorKeys;
    private spaceKey: Input.Keyboard.Key;
    private enterKey: Input.Keyboard.Key;
    private isGameOver = false;
    private hits = 0;
    private leaderboard/*: Array<Player>*/;
    private name = '';
    private getName = false;
    private saved = false;
    preload() {
        this.leaderboard = [new Player("one", 1), new Player("two", 2), new Player("three", 3), new Player("four", 4)];
        // Preload images so that we can use them in our game
        this.load.image('space', 'images/deep-space.jpg');
        this.load.image('bullet', 'images/scratch-laser.png');
        this.load.image('ship', 'images/scratch-spaceship.png');
        this.load.image('meteor', 'images/scratch-meteor.png');
    }

    create() {

        if (this.isGameOver) {
            return;
        }

        //  Add a background
        this.add.tileSprite(0, 0, this.game.canvas.width, this.game.canvas.height, 'space').setOrigin(0, 0);
        this.gameoverText = this.add.text(this.game.canvas.width / 2, this.game.canvas.height / 2, "",
            { font: "65px Arial", fill: "#ff0044", align: "center" });
        this.points = this.add.text(this.game.canvas.width * 0.1, this.game.canvas.height * 0.1, "0",
            { font: "32px Arial", fill: "#ff0044", align: "left" });

        // Create bullets and meteors
        this.bullets = this.physics.add.group({
            classType: Bullet,
            maxSize: 10,
            runChildUpdate: true
        });
        this.meteors = this.physics.add.group({
            classType: Meteor,
            maxSize: 20,
            runChildUpdate: true
        });

        // Add the sprite for our space ship.
        this.spaceShip = new Spaceship(this);
        this.physics.add.existing(this.children.add(this.spaceShip));

        // Position the spaceship horizontally in the middle of the screen
        // and vertically at the bottom of the screen.
        this.spaceShip.setPosition(this.game.canvas.width / 2, this.game.canvas.height * 0.9);

        // Setup game input handling
        this.cursors = this.input.keyboard.createCursorKeys();
        this.input.keyboard.addCapture([' ']);
        this.enterKey = this.input.keyboard.addKey(Input.Keyboard.KeyCodes.ENTER);
        this.spaceKey = this.input.keyboard.addKey(Input.Keyboard.KeyCodes.SPACE);

        this.physics.add.collider(this.bullets, this.meteors, (bullet: Bullet, meteor: Meteor) => {
            if (meteor.active && bullet.active) {
                this.points.setText((++this.hits).toString())
                meteor.kill();
                bullet.kill();
            }
        }, null, this);
        this.physics.add.collider(this.spaceShip, this.meteors, this.gameOver, null, this);
    }

    update(_, delta: number) {
        if (this.isGameOver) {
            if (this.enterKey.isDown) {
                this.showInputName();
            }
        } else {
            // Move ship if cursor keys are pressed
            if (this.cursors.left.isDown) {
                this.spaceShip.move(delta, Direction.Left);
            }
            else if (this.cursors.right.isDown) {
                this.spaceShip.move(delta, Direction.Right);
            }

            if (this.spaceKey.isDown) {
                this.fireBullet();
            }
            this.handleMeteors();
        }

    }


    /*
  keyDown(event: KeyboardEvent) { // with type info
      this.gameoverText.setText("\nkeyPressed\n");
      //this.gameoverText.setText((event.target as HTMLInputElement).value + ' | ');
    }*/
    delay(ms: number) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }
    async showInputName() {
        if (!this.getName) {
            this.gameoverText.setText("Enter your initials\n(3 letters)\n"+this.name);
            this.getName = true;
            document.addEventListener('keyup', (event) => {
                if (!this.saved) {
                    const keyName = event.key;

                    // As the user release the Ctrl key, the key is no longer active.
                    // So event.ctrlKey is false.

                    if (keyName == 'Enter') {
                        if (this.name != '') {
                            this.saved = true;
                            this.leaderboard.push(new Player(this.name, this.hits));
                            this.showLeaderboard();
                        }
                    } else {
                        if (keyName != undefined && keyName.length <= 1 && this.name.length<3) {
                            this.name += keyName;
                        }
                        this.gameoverText.setText("Enter your initials\n(3 letters)\n"+this.name);
                    }
                }
            }, false);
        }
        //this.getName=true;
        //while(this.getName){
        //await this.delay(500);
        //}
        //this.showLeaderboard();
    }
    async showLeaderboard() {
        /*grecaptcha.ready(function() {
            grecaptcha.execute('reCAPTCHA_site_key', {action: 'homepage'}).then(function(token) {*/
                await axios.post('http://localhost:5000/api/Player/' + this.name + '/' + this.hits);
                let getPlayer;
                try {
                    getPlayer = await axios.get('http://localhost:5000/api/Player/');
                } catch (error) {
                    console.error(error)
                }
                console.log(getPlayer.data);
                this.leaderboard = getPlayer.data;
        
                this.leaderboard.sort(new Player("", 0).comparer);
                let text = this.leaderboard[0].name + " " + this.leaderboard[0].score;
                for (let i = 1; i < this.leaderboard.length; i++) {
                    text += "\n" + this.leaderboard[i].name + " " + this.leaderboard[i].score;
                }
                this.gameoverText.setText(text);
                this.gameoverText.setFontSize(32);
                //this.gameoverText.setText(getPlayer);
            /*}
        });*/
    }
    fireBullet() {
        if (this.time.now > this.bulletTime) {
            // Find the first unused (=unfired) bullet
            const bullet = this.bullets.get() as Bullet;
            if (bullet) {
                bullet.fire(this.spaceShip.x, this.spaceShip.y);
                this.bulletTime = this.time.now + 100;
            }
        }
    }
    handleMeteors() {
        // Check if it is time to launch a new meteor.
        if (this.time.now > this.meteorTime) {
            // Find first meteor that is currently not used
            const meteor = this.meteors.get() as Meteor;
            if (meteor) {
                meteor.fall();
                this.meteorTime = this.time.now + 500 + 1000 * Math.random();
            }
        }
    }

    gameOver() {
        if (!this.isGameOver) {
            this.isGameOver = true;
            this.gameoverText.setText("Game Over :-(\npress enter");
        }

        this.bullets.getChildren().forEach((b: Bullet) => b.kill());
        this.meteors.getChildren().forEach((m: Meteor) => m.kill());
        this.spaceShip.kill();

        // Display "game over" text

        this.gameoverText.setOrigin(0.5, 0.5);
    }
}

const config = {
    type: CANVAS,
    width: 512,
    height: 512,
    scene: [ShooterScene],
    physics: { default: 'arcade' },
    audio: { noAudio: true }
};

new Game(config);
