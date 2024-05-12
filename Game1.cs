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
    
    double fadeTargetTime = 1;
    double fadeTimer = 0;

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

    }

    protected override void Initialize()
    {
        GameTime gt = new GameTime();
        bs = BasicState.Splash;
        _color_fade = new Color(255, 255, 255, 255);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _texture_splash = Content.Load<Texture2D>("SplashScreen");
        _texture_title = Content.Load<Texture2D>("TitleScreen");
        _texture_current = _texture_splash;
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if(bs == BasicState.Splash)
        {
            if(fadeTimer < fadeTargetTime)
            {
                fadeTimer += gameTime.ElapsedGameTime.TotalSeconds;
                //Debug.WriteLine("fadeTimer: " + fadeTimer);
                _color_fade.R = (byte)(255 - 255 * (fadeTimer / fadeTargetTime));
                _color_fade.G = (byte)(255 - 255 * (fadeTimer / fadeTargetTime));
                _color_fade.B = (byte)(255 - 255 * (fadeTimer / fadeTargetTime));
            }
            else
            {
                fadeTimer = 0;
                 _texture_current = _texture_title;
                bs = BasicState.Title;
            }
        }
        else if(bs == BasicState.Title)
        {
            if(fadeTimer < fadeTargetTime)
            {
                fadeTimer += gameTime.ElapsedGameTime.TotalSeconds;
                //Debug.WriteLine("fadeTimer: " + fadeTimer);
                _color_fade.R = (byte)(255 * (fadeTimer / fadeTargetTime));
                _color_fade.G = (byte)(255 * (fadeTimer / fadeTargetTime));
                _color_fade.B = (byte)(255 * (fadeTimer / fadeTargetTime));
            }
            else{
                bs = BasicState.Game;
            }
        }
        else{
            if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                fadeTimer = 0;
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

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
