using System;
using System.Collections.Generic;
using System.Diagnostics;
//using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lifeblood;

public class TitleScene: IScene
{
    Boolean isPlaying;
    
    Texture2D _texture_splash;
    Texture2D _texture_title;
    Texture2D _texture_pointer;
    Texture2D _texture_screen_mask;
    Rectangle fade_rectangle;
    int mask_alpha;
    Boolean isFading;

    SpriteFont _font;
    
    List<Texture2D> draw_list_textures;
    List<Rectangle> textures_rectangles;
    List<Color> textures_colors;    

    List<SpriteFont> draw_list_fonts;
    List<String> fonts_strings;
    List<Vector2> fonts_positions;
    List<Color> fonts_colors;

    double delayTimer;
    double delayTarget;

    int pointerOffset;
    Boolean isDownPressed;
    Boolean isUpPressed;
    Boolean isEnterPressed;

    enum TitleStates
    {
        FadeIn1,
        Splash,
        Fadeout1,
        FadeIn2,
        Title,
        FadeOut2
    }
    TitleStates ts;

    bool IScene.IsPlaying()
    {
        return isPlaying;
    }

    void IScene.LoadContent(ContentManager content)
    {
        isPlaying = true;

        _texture_splash = content.Load<Texture2D>("SplashScreen");
        _texture_title = content.Load<Texture2D>("TitleScreen");
        _texture_pointer = content.Load<Texture2D>("TitlePointer");
        _texture_screen_mask = content.Load<Texture2D>("mask");
        _font = content.Load<SpriteFont>("alagard");

        mask_alpha = 255;
        fade_rectangle = new Rectangle(0, 0, 256, 244);
        isFading = true;

        draw_list_textures = new List<Texture2D>();
        textures_rectangles = new List<Rectangle>();
        textures_colors = new List<Color>();

        draw_list_textures.Add(_texture_splash);
        textures_rectangles.Add(new Rectangle(0, 0, 256, 244));
        textures_colors.Add(Color.White);

        draw_list_fonts = new List<SpriteFont>();
        fonts_strings = new List<String>();
        fonts_positions = new List<Vector2>();
        fonts_colors = new List<Color>();

        delayTimer = 0;
        delayTarget = 2;

        pointerOffset = 0;
        isDownPressed = false;
        isUpPressed = false;
        isEnterPressed = false;

        ts = TitleStates.FadeIn1;
    }

    void IScene.Update(GameTime gameTime)
    {

        switch(ts)
        {
            case TitleStates.FadeIn1:
                if(mask_alpha > 1)
                {
                    mask_alpha -= 2;
                }
                else 
                {
                    isFading = false;
                    ts = TitleStates.Splash;
                }
                break;
            
            case TitleStates.Splash:
                if(delayTimer < delayTarget)
                {
                    delayTimer += gameTime.ElapsedGameTime.TotalSeconds;
                }
                else 
                {   
                    isFading = true;
                    ts = TitleStates.Fadeout1;
                }
                break;

            case TitleStates.Fadeout1:
                if(mask_alpha < 254)
                {
                    mask_alpha += 2;
                }
                else 
                {
                    int r_index = draw_list_textures.IndexOf(_texture_splash);
                    draw_list_textures.RemoveAt(r_index);
                    textures_rectangles.RemoveAt(r_index);
                    textures_colors.RemoveAt(r_index);

                    draw_list_fonts.Insert(0, _font);
                    draw_list_fonts.Insert(0,_font);
                    fonts_strings.Insert(0,"New Game");
                    fonts_strings.Insert(0,"Continue");
                    fonts_positions.Insert(0, new Vector2(256/3+10,244/2+20));
                    fonts_positions.Insert(0, new Vector2(256/3+10,244/2+40));
                    fonts_colors.Insert(0, Color.White);
                    fonts_colors.Insert(0, Color.Gray);

                    draw_list_textures.Insert(0,_texture_title);
                    textures_rectangles.Insert(0,new Rectangle(0, 0, 256, 244));
                    textures_colors.Insert(0,Color.White);

                    ts = TitleStates.FadeIn2;
                }
                break;

            case TitleStates.FadeIn2:
                if(mask_alpha > 1)
                {
                    mask_alpha -= 2;
                }
                else 
                {
                    isFading = false;

                    draw_list_textures.Insert(1,_texture_pointer);
                    textures_rectangles.Insert(1, new Rectangle(256/4,244/2+17,20,20));
                    textures_colors.Insert(1,Color.White);

                    ts = TitleStates.Title;
                }

                break;

            case TitleStates.Title:
                
                if(!isDownPressed && Keyboard.GetState().IsKeyDown(Keys.Down) && pointerOffset == 0)
                {
                    isDownPressed = true;
                    pointerOffset += 20;
                    int r_index = draw_list_textures.IndexOf(_texture_pointer);
                    int my_X = textures_rectangles[r_index].X;
                    int my_Y = textures_rectangles[r_index].Y + pointerOffset;
                    int my_W = textures_rectangles[r_index].Width;
                    int my_H = textures_rectangles[r_index].Height;
                    textures_rectangles[r_index] = new Rectangle(my_X, my_Y, my_W, my_H);
                }
                if(Keyboard.GetState().IsKeyUp(Keys.Down))
                {
                    isDownPressed = false;
                }

                if(!isUpPressed && Keyboard.GetState().IsKeyDown(Keys.Up) && pointerOffset != 0)
                {
                    isUpPressed = true;
                    int r_index = draw_list_textures.IndexOf(_texture_pointer);
                    int my_X = textures_rectangles[r_index].X;
                    int my_Y = textures_rectangles[r_index].Y - pointerOffset;
                    pointerOffset -= 20;
                    int my_W = textures_rectangles[r_index].Width;
                    int my_H = textures_rectangles[r_index].Height;
                    textures_rectangles[r_index] = new Rectangle(my_X, my_Y, my_W, my_H);
                }
                if(Keyboard.GetState().IsKeyUp(Keys.Up))
                {
                    isUpPressed = false;
                }

                if(!isEnterPressed && Keyboard.GetState().IsKeyDown(Keys.Enter) && pointerOffset == 0)
                {
                    isEnterPressed = true;
                    isFading = true;
                    ts = TitleStates.FadeOut2;
                }
                if(Keyboard.GetState().IsKeyUp(Keys.Enter))
                {
                    isEnterPressed = false;
                }

                break;

            case TitleStates.FadeOut2:
                if(mask_alpha < 254)
                {
                    mask_alpha += 2;
                }
                else 
                {
                    isPlaying = false;
                }
                break;
        }
    }

    void IScene.Draw(GameTime gameTime, SpriteBatch spriteBatch, int scale_factor)
    {
        spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
        for(int i = 0; i < draw_list_textures.Count; i++) 
        {
            spriteBatch.Draw(
                draw_list_textures[i], 
                new Rectangle(
                    textures_rectangles[i].X*scale_factor,
                    textures_rectangles[i].Y*scale_factor,
                    textures_rectangles[i].Width*scale_factor,
                    textures_rectangles[i].Height*scale_factor), 
                textures_colors[i]); 
        }

        for(int i = 0; i < draw_list_fonts.Count; i++)
        {
            spriteBatch.DrawString(
                draw_list_fonts[i], 
                fonts_strings[i], 
                new Vector2(
                    fonts_positions[i].X*scale_factor, 
                    fonts_positions[i].Y*scale_factor), 
                fonts_colors[i], 
                0, new Vector2(0,0), new Vector2(3,3), 0, 0, false);
        }

        if(isFading)
        {
            spriteBatch.Draw(
                _texture_screen_mask, 
                new Rectangle(
                    fade_rectangle.X, 
                    fade_rectangle.Y, 
                    fade_rectangle.Width*scale_factor, 
                    fade_rectangle.Height*scale_factor),
                new Color(
                    255,
                    255,
                    255,
                    mask_alpha));
        }

        spriteBatch.End();
    }

    internal IScene Load()
    {
        throw new NotImplementedException();
    }
}