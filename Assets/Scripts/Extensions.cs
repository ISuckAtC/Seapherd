using System;
public static class Extensions
{
    public static void SetStatus(this (GameManager.MissionStatus status, object extras) tuple, GameManager.MissionStatus status)
    {
        tuple = (status, tuple.extras);
    }
    public static void SetExtras(this (GameManager.MissionStatus status, object extras) tuple, object extras)
    {
        tuple = (tuple.status, extras);
    }
}