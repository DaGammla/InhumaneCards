using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Threading.Tasks;
using static Retyped.dom;

namespace InhumaneWeb{
    public class WebApp{
        public static WebGame webGame;

        public static uint GAME_CANVAS_X;
        public static uint GAME_CANVAS_Y;

        public static void Main(){

            GAME_CANVAS_X = (uint)window.innerWidth - 5;
            GAME_CANVAS_Y = (uint)window.innerHeight - 5;

            var canvas = new HTMLCanvasElement();
            canvas.width = GAME_CANVAS_X;
            canvas.height = GAME_CANVAS_Y;
            canvas.id = "monogamecanvas";

            var styles = new HTMLStyleElement();
            styles.innerHTML = @"
#monogamecanvas {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0,0,0,0);
  cursor: pointer;
}
#overlay {
  position: fixed;
  display: none;
  width: 100%;
  height: 100%;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0,0,0,0);
  z-index: 2;
  cursor: pointer;
}
#text{
  position: absolute;
  top: 50%;
  left: 50%;
  font-size: 50px;
  color: white;
  transform: translate(-50%,-50%);
  -ms-transform: translate(-50%,-50%);
}";

            var overlay = new HTMLDivElement();
            overlay.id = "overlay";

            var loadingText = new HTMLDivElement();
            loadingText.innerHTML = "Loading...";
            loadingText.id = "text";

            overlay.appendChild(loadingText);

            document.body.appendChild(styles);
            document.body.appendChild(canvas);
            document.body.appendChild(overlay);

            document.body.style.backgroundColor = $"rgb({InhumaneCards.InhumaneGame.CLEAR_COLOR.R}, {InhumaneCards.InhumaneGame.CLEAR_COLOR.G}, {InhumaneCards.InhumaneGame.CLEAR_COLOR.B})";

            overlay.style.display = "block";
           
            Window.addEventListenerFn<string> resizedAction = (eventStr) => {

                GAME_CANVAS_X = (uint)window.innerWidth - 50;
                GAME_CANVAS_Y = (uint)window.innerHeight - 50;

                canvas.width = GAME_CANVAS_X;
                canvas.height = GAME_CANVAS_Y;

                webGame.SetScreenSize(GAME_CANVAS_X, GAME_CANVAS_Y);
            };

			//window.addEventListener("resize", resizedAction);

            window.setTimeout((_) => {
                webGame = new WebGame();
                webGame.Run();
                overlay.style.display = "none";
            }, 10);

            
        }

        public static void PerformStringInput(string question, string defaultText, Action<string> onDoneAction) {
            
            window.setTimeout((_) => {
                onDoneAction(window.prompt(question, defaultText));
            }, 10);
        }
    }
}
