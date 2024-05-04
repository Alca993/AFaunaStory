using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace DamagePackage
{
    public enum DamageType { Bullet, Rocket}

    public class Damage
    {
        private const char NEW_LINE = '\n';
        private const char EQUALS = '=';

        private DamageType type;
        private string description;
        private float amount;

        public Damage()
        {
        }

        public Damage(DamageType type, string description, float amount)
        {
            this.type = type;
            this.description = description;
            this.amount = amount;
        }

        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                this.description = value;
            }
        }

        public DamageType Type
        {
            get
            {
                return type;
            }

            set
            {
                this.type = value;
            }
        }

        public float Amount
        {
            get
            {
                return amount;
            }

            set
            {
                this.amount = value;
            }
        }

        public static Damage ReadDamage(string filePath)
        {
            DamageType type = DamageType.Bullet;
            string description = "";
            float amount = 0;

            TextAsset data = Resources.Load<TextAsset>(filePath);
            string[] lines = data.text.Split(NEW_LINE);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] token = line.Split(EQUALS);

                //ipotizzo formattazione del file e contenuto singolo
                switch (token[0])
                {
                    case "type":
                        type = (DamageType)Enum.Parse(typeof(DamageType), token[1]);
                        break;
                    case "description":
                        description = token[1];
                        break;
                    case "amount":
                        amount = float.Parse(token[1], CultureInfo.InvariantCulture);
                        break;
                    default:
                        break;
                }

            }
            return new Damage(type, description, amount);

        }

    }
}