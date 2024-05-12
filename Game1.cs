using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lifeblood;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private int scale_facctor = 3;
    Texture2D _texture_splash;
    Texture2D _texture_title;
    Texture2D _texture_current;
    Color _color_fade;
    SpriteFont _font;
    int scale_titleFont = 4;
    Boolean isTitleDraw;

    double fadeInTargetTime = 1;
    double fadeInTimer = 0;
    double delayTimer = 0;
    double delayTarget = 1.5;
    double fadeOutTargetTime = 1;
    double fadeOutTimer = 0;

    enum BasicState
    {
        Splash,
        Title,
        Game,
        Exit
    }
    BasicState bs;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 256*scale_facctor;
        _graphics.PreferredBackBufferHeight = 244*scale_facctor;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        isTitleDraw = false;
    }

    protected override void Initialize()
    {
        GameTime gt = new GameTime();
        bs = BasicState.Splash;
        _color_fade = new Color(0, 0, 0, 255);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _texture_splash = Content.Load<Texture2D>("SplashScreen");
        _texture_title = Content.Load<Texture2D>("TitleScreen");
        _texture_current = _texture_splash;
        _font = Content.Load<SpriteFont>("alagard");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if(bs == BasicState.Splash)
        {
            if(fadeInTimer < fadeInTargetTime)
            {
                fadeInTimer += gameTime.ElapsedGameTime.TotalSeconds;
                _color_fade.R = (byte)(255 * (fadeInTimer / fadeInTargetTime));
                _color_fade.G = (byte)(255 * (fadeInTimer / fadeInTargetTime));
                _color_fade.B = (byte)(255 * (fadeInTimer / fadeInTargetTime));
            }
            else if(delayTimer < delayTarget)
            {
                delayTimer += gameTime.ElapsedGameTime.TotalSeconds;    
            }
            else if(fadeOutTimer < fadeOutTargetTime)
            {
                fadeOutTimer += gameTime.ElapsedGameTime.TotalSeconds;
                _color_fade.R = (byte)(255 - 255 * (fadeOutTimer / fadeOutTargetTime));
                _color_fade.G = (byte)(255 - 255 * (fadeOutTimer / fadeOutTargetTime));
                _color_fade.B = (byte)(255 - 255 * (fadeOutTimer / fadeOutTargetTime));
            }
            else
            {
                fadeInTimer = 0;
                fadeOutTimer = 0;
                 _texture_current = _texture_title;
                bs = BasicState.Title;
            }
        }
        else if(bs == BasicState.Title)
        {
            if(fadeInTimer < fadeInTargetTime)
            {
                fadeInTimer += gameTime.ElapsedGameTime.TotalSeconds;
                _color_fade.R = (byte)(255 * (fadeInTimer / fadeInTargetTime));
                _color_fade.G = (byte)(255 * (fadeInTimer / fadeInTargetTime));
                _color_fade.B = (byte)(255 * (fadeInTimer / fadeInTargetTime));
            }
            else{
                bs = BasicState.Game;
                isTitleDraw = true;
            }
        }
        else{
            if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                isTitleDraw = false;
                fadeOutTimer = 0;
                _texture_current = _texture_splash;
                bs = BasicState.Splash;
            }
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

        _spriteBatch.Draw(_texture_current, new Rectangle(0,0,256*scale_facctor,244*scale_facctor), _color_fade);
        if(isTitleDraw)
        {
            _spriteBatch.DrawString(_font, "New Game", new Vector2(256*scale_facctor/3, 244*scale_facctor/2), Color.White, 0, new Vector2(0,0), scale_titleFont, 0, 0);
        }
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
