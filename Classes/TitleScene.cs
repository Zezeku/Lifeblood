using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lifeblood;

public class TitleScene: IScene
{
    Controller controller;
    
    Texture2D _texture_splash;
    Texture2D _texture_title;
    Texture2D _texture_pointer;

    Boolean isTitle;
    SpriteFont _font;
    
    List<Texture2D> draw_list_textures;
    List<SpriteFont> draw_list_fonts;
    List<String> draw_list_strings;

    double delayTimer;
    double delayTarget;

    public TitleScene(Controller controller)
    {
        this.controller = controller;
    }

    void IScene.LoadContent(ContentManager content)
    {
        controller.isActive = false; //default for fadeIn

        isTitle = false;

        _texture_splash = content.Load<Texture2D>("SplashScreen");
        _texture_title = content.Load<Texture2D>("TitleScreen");
        _texture_pointer = content.Load<Texture2D>("TitlePointer");
        _font = content.Load<SpriteFont>("alagard");

        draw_list_textures = new List<Texture2D>();
        draw_list_textures.Add(_texture_splash);

        draw_list_fonts = new List<SpriteFont>();
        draw_list_strings = new List<String>();

        delayTimer = 0;
        delayTarget = 1;
    }

    void IScene.Update(GameTime gameTime)
    {
        if(delayTimer < delayTarget)
        {
            delayTimer += gameTime.ElapsedGameTime.TotalSeconds;
        }
        else if(!isTitle) 
        {
            isTitle = true;
            draw_list_textures.Remove(_texture_splash);
            draw_list_textures.Add(_texture_title);
            draw_list_fonts.Add(_font);
            draw_list_fonts.Add(_font);
            draw_list_strings.Add("New Game");
            draw_list_strings.Add("Continue");
        }
        else 
        {

        }
    }

    void IScene.Draw(GameTime gameTime, SpriteBatch spriteBatch, int scale_factor)
    {
        foreach (var texture in draw_list_textures)
            spriteBatch.Draw(texture, new Rectangle(0,0,256*scale_factor,244*scale_factor), Color.White); 

        for(int i = 0; i < draw_list_fonts.Count; i++)
        {
            spriteBatch.DrawString(draw_list_fonts[i], draw_list_strings[i], new Vector2(50*scale_factor,50*scale_factor*(i+1)), Color.Black, 0, new Vector2(0,0), new Vector2(3,3), 0, 0, false);
        }

        spriteBatch.End();
    }
}