/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using System;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows;
using System.Windows.Media;

namespace Forgotten_Land_Launcher.Library
{
    internal class Animations
    {
        /// <summary>
        /// Animate fade in framework element synchronously
        /// </summary>
        /// <param name="element"></param>
        /// <param name="millisecondsDuration"></param>
        public static void FadeIn(FrameworkElement element, int millisecondsDuration)
        {
            element.Visibility = Visibility.Visible;

            // Animate opacity
            Storyboard storyboard = new Storyboard();
            DoubleAnimation FadeInAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(millisecondsDuration),
                From = 0,
                To = 1
            };
            Storyboard.SetTarget(FadeInAnimation, element);
            Storyboard.SetTargetProperty(FadeInAnimation, new PropertyPath(UIElement.OpacityProperty));
            storyboard.Children.Add(FadeInAnimation);
            storyboard.Begin();
        }

        /// <summary>
        /// Animate fade in framework element asynchronously
        /// </summary>
        /// <param name="element"></param>
        /// <param name="millisecondsDuration"></param>
        /// <returns></returns>
        public static Task FadeInAsync(FrameworkElement element, int millisecondsDuration)
        {
            element.Visibility = Visibility.Visible;

            // Create a task completion source to signal when the animation is completed
            var tcs = new TaskCompletionSource<bool>();

            // Animate opacity
            Storyboard storyboard = new Storyboard();
            DoubleAnimation FadeInAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(millisecondsDuration),
                From = 0,
                To = 1
            };
            storyboard.Children.Add(FadeInAnimation);
            Storyboard.SetTarget(FadeInAnimation, element);
            Storyboard.SetTargetProperty(FadeInAnimation, new PropertyPath(UIElement.OpacityProperty));

            // Hook up an event handler for when the animation completes
            FadeInAnimation.Completed += (sender, e) =>
            {
                tcs.SetResult(true); // Signal completion
            };

            storyboard.Begin();

            return tcs.Task;
        }

        /// <summary>
        /// Animate fade out framework element synchronously
        /// </summary>
        /// <param name="element"></param>
        /// <param name="millisecondsDuration"></param>
        public static async void FadeOut(FrameworkElement element, int millisecondsDuration)
        {
            // Animate opacity
            Storyboard storyboard = new Storyboard();
            DoubleAnimation FadeOutAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(millisecondsDuration),
                From = 1,
                To = 0
            };
            Storyboard.SetTarget(FadeOutAnimation, element);
            Storyboard.SetTargetProperty(FadeOutAnimation, new PropertyPath(UIElement.OpacityProperty));
            storyboard.Children.Add(FadeOutAnimation);
            storyboard.Begin();

            await Task.Delay(millisecondsDuration);
            element.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Animate fade out framework element asynchronously
        /// </summary>
        /// <param name="element"></param>
        /// <param name="millisecondsDuration"></param>
        /// <returns></returns>
        public static Task FadeOutAsync(FrameworkElement element, int millisecondsDuration)
        {
            element.Visibility = Visibility.Visible;

            // Create a task completion source to signal when the animation is completed
            var tcs = new TaskCompletionSource<bool>();

            // Animate opacity
            Storyboard storyboard = new Storyboard();
            DoubleAnimation FadeInAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(millisecondsDuration),
                From = 1,
                To = 0
            };
            storyboard.Children.Add(FadeInAnimation);
            Storyboard.SetTarget(FadeInAnimation, element);
            Storyboard.SetTargetProperty(FadeInAnimation, new PropertyPath(UIElement.OpacityProperty));

            // Hook up an event handler for when the animation completes
            FadeInAnimation.Completed += (sender, e) =>
            {
                tcs.SetResult(true); // Signal completion
            };

            storyboard.Begin();

            return tcs.Task;
        }

        /// <summary>
        /// Animate moving up and fade in framework element synchronously
        /// </summary>
        /// <param name="element"></param>
        /// <param name="opacityDuration"></param>
        /// <param name="moveDuration"></param>
        public static void MoveUpAndFadeIn(FrameworkElement element, int opacityDuration, int moveDuration)
        {
            // Animate opacity
            element.Visibility = Visibility.Visible;

            Storyboard storyboard = new Storyboard();
            DoubleAnimation FadeInAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(opacityDuration),
                From = 0,
            };
            Storyboard.SetTarget(FadeInAnimation, element);
            Storyboard.SetTargetProperty(FadeInAnimation, new PropertyPath(UIElement.OpacityProperty));
            storyboard.Children.Add(FadeInAnimation);
            storyboard.Begin();

            // Animate position
            TranslateTransform trans = new TranslateTransform();
            element.RenderTransform = trans;
            DoubleAnimation moveInUpAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(moveDuration),
                From = 10,
            };

            trans.BeginAnimation(TranslateTransform.YProperty, moveInUpAnimation);
        }

        /// <summary>
        /// Animate moving left and fade out framework element synchronously
        /// </summary>
        /// <param name="element"></param>
        /// <param name="opacityDuration"></param>
        /// <param name="moveDuration"></param>
        /// <param name="moveAmount"></param>
        public static void MoveLeftAndFadeOut(FrameworkElement element, int opacityDuration, int moveDuration, int moveAmount)
        {
            // Animate opacity
            Storyboard storyboard = new Storyboard();
            DoubleAnimation FadeOutAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(opacityDuration),
                To = 0,
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }, // Use QuadraticEase for a slow start and speed up
            };
            Storyboard.SetTarget(FadeOutAnimation, element);
            Storyboard.SetTargetProperty(FadeOutAnimation, new PropertyPath(UIElement.OpacityProperty));
            storyboard.Children.Add(FadeOutAnimation);

            // Animate position
            TranslateTransform trans = new TranslateTransform();
            element.RenderTransform = trans;
            DoubleAnimation moveOutLeftAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(moveDuration),
                To = moveAmount,
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }, // Use QuadraticEase for a slow start and speed up
            };

            trans.BeginAnimation(TranslateTransform.XProperty, moveOutLeftAnimation);

            // Start the storyboard
            storyboard.Begin();
        }

        /// <summary>
        /// Animate moving right and fade out framework element synchronously
        /// </summary>
        /// <param name="element"></param>
        /// <param name="opacityDuration"></param>
        /// <param name="moveDuration"></param>
        /// <param name="moveAmount"></param>
        public static void MoveRightAndFadeOut(FrameworkElement element, int opacityDuration, int moveDuration, int moveAmount)
        {
            // Animate opacity
            Storyboard storyboard = new Storyboard();
            DoubleAnimation FadeOutAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(opacityDuration),
                To = 0,
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }, // Use QuadraticEase for a slow start and speed up
            };
            Storyboard.SetTarget(FadeOutAnimation, element);
            Storyboard.SetTargetProperty(FadeOutAnimation, new PropertyPath(UIElement.OpacityProperty));
            storyboard.Children.Add(FadeOutAnimation);

            // Animate position
            TranslateTransform trans = new TranslateTransform();
            element.RenderTransform = trans;
            DoubleAnimation moveOutLeftAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(moveDuration),
                To = moveAmount,
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }, // Use QuadraticEase for a slow start and speed up
            };

            trans.BeginAnimation(TranslateTransform.XProperty, moveOutLeftAnimation);

            // Start the storyboard
            storyboard.Begin();
        }

        /// <summary>
        /// Animate moving down and fade in framework element synchronously
        /// </summary>
        /// <param name="element"></param>
        /// <param name="opacityDuration"></param>
        /// <param name="moveDuration"></param>
        /// <param name="fromAmount"></param>
        public static void MoveDownAndFadeIn(FrameworkElement element, int opacityDuration, int moveDuration, int fromAmount)
        {
            element.Opacity = 0;
            element.Visibility = Visibility.Visible;

            // Animate opacity
            Storyboard storyboard = new Storyboard();
            DoubleAnimation FadeInAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(opacityDuration),
                To = 1, // Fade in by setting To to 1
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }, // Use QuadraticEase for a smooth start and slow down
            };
            Storyboard.SetTarget(FadeInAnimation, element);
            Storyboard.SetTargetProperty(FadeInAnimation, new PropertyPath(UIElement.OpacityProperty));
            storyboard.Children.Add(FadeInAnimation);

            // Animate position to move back in
            TranslateTransform trans = new TranslateTransform();
            element.RenderTransform = trans;
            DoubleAnimation moveBackRightAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(moveDuration),
                From = fromAmount,
                To = 0, // Move back to the original position (moveAmount)
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }, // Use QuadraticEase for a smooth start and slow down
            };

            trans.BeginAnimation(TranslateTransform.YProperty, moveBackRightAnimation);

            // Start the storyboard
            storyboard.Begin();
        }

        /// <summary>
        /// Animate moving right and fade in framework element synchronously
        /// </summary>
        /// <param name="element"></param>
        /// <param name="opacityDuration"></param>
        /// <param name="moveDuration"></param>
        /// <param name="fromAmount"></param>
        public static void MoveRightAndFadeIn(FrameworkElement element, int opacityDuration, int moveDuration, int fromAmount)
        {
            element.Opacity = 0;
            element.Visibility = Visibility.Visible;

            // Animate opacity
            Storyboard storyboard = new Storyboard();
            DoubleAnimation FadeInAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(opacityDuration),
                To = 1, // Fade in by setting To to 1
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }, // Use QuadraticEase for a smooth start and slow down
            };
            Storyboard.SetTarget(FadeInAnimation, element);
            Storyboard.SetTargetProperty(FadeInAnimation, new PropertyPath(UIElement.OpacityProperty));
            storyboard.Children.Add(FadeInAnimation);

            // Animate position to move back in
            TranslateTransform trans = new TranslateTransform();
            element.RenderTransform = trans;
            DoubleAnimation moveBackRightAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(moveDuration),
                From = fromAmount,
                To = 0, // Move back to the original position (moveAmount)
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }, // Use QuadraticEase for a smooth start and slow down
            };

            trans.BeginAnimation(TranslateTransform.XProperty, moveBackRightAnimation);

            // Start the storyboard
            storyboard.Begin();
        }

        /// <summary>
        /// Animate moving left and fade in framework element synchronously
        /// </summary>
        /// <param name="element"></param>
        /// <param name="opacityDuration"></param>
        /// <param name="moveDuration"></param>
        /// <param name="fromAmount"></param>
        public static void MoveLeftAndFadeIn(FrameworkElement element, int opacityDuration, int moveDuration, int fromAmount)
        {
            element.Opacity = 0;
            element.Visibility = Visibility.Visible;

            // Animate opacity
            Storyboard storyboard = new Storyboard();
            DoubleAnimation FadeInAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(opacityDuration),
                To = 1, // Fade in by setting To to 1
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }, // Use QuadraticEase for a smooth start and slow down
            };
            Storyboard.SetTarget(FadeInAnimation, element);
            Storyboard.SetTargetProperty(FadeInAnimation, new PropertyPath(UIElement.OpacityProperty));
            storyboard.Children.Add(FadeInAnimation);

            // Animate position to move back in
            TranslateTransform trans = new TranslateTransform();
            element.RenderTransform = trans;
            DoubleAnimation moveBackRightAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(moveDuration),
                From = fromAmount,
                To = 0, // Move back to the original position (moveAmount)
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }, // Use QuadraticEase for a smooth start and slow down
            };

            trans.BeginAnimation(TranslateTransform.XProperty, moveBackRightAnimation);

            // Start the storyboard
            storyboard.Begin();
        }

        /// <summary>
        /// Animate fade in and out framework element synchronously
        /// </summary>
        /// <param name="element"></param>
        /// <param name="millisecondsInterval"></param>
        /// <returns></returns>
        public static Storyboard FadeInAndOutInfinitely(FrameworkElement element, int millisecondsInterval)
        {
            Storyboard storyboard = new Storyboard();

            DoubleAnimation FadeOutAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(millisecondsInterval),
                From = 0,
                To = 1,
                RepeatBehavior = RepeatBehavior.Forever,
                AutoReverse = true
            };

            Storyboard.SetTarget(FadeOutAnimation, element);
            Storyboard.SetTargetProperty(FadeOutAnimation, new PropertyPath(UIElement.OpacityProperty));
            storyboard.Children.Add(FadeOutAnimation);
            storyboard.Begin();

            return storyboard;
        }
    }
}
