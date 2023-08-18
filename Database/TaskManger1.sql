PGDMP     9                    {            TaskManager    15.3    15.3 )    2           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            3           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            4           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            5           1262    16444    TaskManager    DATABASE     �   CREATE DATABASE "TaskManager" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'English_United States.utf8';
    DROP DATABASE "TaskManager";
                postgres    false                        3079    16523 	   adminpack 	   EXTENSION     A   CREATE EXTENSION IF NOT EXISTS adminpack WITH SCHEMA pg_catalog;
    DROP EXTENSION adminpack;
                   false            6           0    0    EXTENSION adminpack    COMMENT     M   COMMENT ON EXTENSION adminpack IS 'administrative functions for PostgreSQL';
                        false    2            �            1259    16451    Channels    TABLE       CREATE TABLE public."Channels" (
    "Id" integer NOT NULL,
    "Name" text NOT NULL,
    "Description" character varying(255),
    "CreationDate" timestamp without time zone,
    "CreatedBy" integer,
    "LastUpdateDate" timestamp without time zone,
    "LastUpdatedBy" integer
);
    DROP TABLE public."Channels";
       public         heap    postgres    false            �            1259    16450    Channels_Id_seq    SEQUENCE     �   ALTER TABLE public."Channels" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Channels_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    217            �            1259    16459    Projects    TABLE       CREATE TABLE public."Projects" (
    "Id" integer NOT NULL,
    "Name" character varying(255) NOT NULL,
    "Status" character varying(255) NOT NULL,
    "Description" character varying(5000),
    "Active" boolean NOT NULL,
    "CreationDate" timestamp without time zone,
    "CreatedBy" integer,
    "LastUpdateDate" timestamp without time zone,
    "LastUpdatedBy" integer,
    "ChannelId" integer DEFAULT 0 NOT NULL,
    "NextAction" character varying(2500),
    "PIC" character varying(255),
    "Progress" integer DEFAULT 0 NOT NULL
);
    DROP TABLE public."Projects";
       public         heap    postgres    false            �            1259    16458    Projects_Id_seq    SEQUENCE     �   ALTER TABLE public."Projects" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Projects_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    219            �            1259    16467    Roles    TABLE     �   CREATE TABLE public."Roles" (
    "Id" integer NOT NULL,
    "Code" character varying(255) NOT NULL,
    "Name" character varying(255) NOT NULL,
    "Description" character varying(255)
);
    DROP TABLE public."Roles";
       public         heap    postgres    false            �            1259    16466    Roles_Id_seq    SEQUENCE     �   ALTER TABLE public."Roles" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Roles_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    221            �            1259    16516    Settings    TABLE     �   CREATE TABLE public."Settings" (
    "Id" integer NOT NULL,
    "UserId" integer NOT NULL,
    "Key" text,
    "Value" character varying(1500)
);
    DROP TABLE public."Settings";
       public         heap    postgres    false            �            1259    16515    Settings_Id_seq    SEQUENCE     �   ALTER TABLE public."Settings" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Settings_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    227            �            1259    16475    Tasks    TABLE     �  CREATE TABLE public."Tasks" (
    "Id" integer NOT NULL,
    "ProjectId" integer NOT NULL,
    "ProjectName" character varying(255),
    "Subject" character varying(500) NOT NULL,
    "Description" character varying(5000),
    "HelpdeskTicket" character varying(255),
    "Status" character varying(255),
    "Progress" character varying(255),
    "PIC" character varying(255),
    "Requestor" character varying(255),
    "Assignee" character varying(255),
    "StartDate" text,
    "DueDate" text,
    "Active" boolean NOT NULL,
    "CreationDate" timestamp without time zone,
    "CreatedBy" integer,
    "LastUpdateDate" timestamp without time zone,
    "LastUpdatedBy" integer,
    "EstimateDate" text,
    "Action" character varying(5000)
);
    DROP TABLE public."Tasks";
       public         heap    postgres    false            �            1259    16474    Tasks_Id_seq    SEQUENCE     �   ALTER TABLE public."Tasks" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Tasks_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    223            �            1259    16483    Users    TABLE     z  CREATE TABLE public."Users" (
    "Id" integer NOT NULL,
    "RoleId" integer NOT NULL,
    "Username" character varying(255) NOT NULL,
    "Password" character varying(255) NOT NULL,
    "DisplayName" character varying(255) NOT NULL,
    "Fullname" character varying(255),
    "Email" character varying(255),
    "Phone" character varying(255),
    "Active" boolean NOT NULL,
    "ChannelId" integer NOT NULL,
    "ChannelName" text,
    "LastLogin" timestamp without time zone,
    "CreationDate" timestamp without time zone,
    "CreatedBy" integer,
    "LastUpdateDate" timestamp without time zone,
    "LastUpdatedBy" integer
);
    DROP TABLE public."Users";
       public         heap    postgres    false            �            1259    16482    Users_Id_seq    SEQUENCE     �   ALTER TABLE public."Users" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Users_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    225            �            1259    16445    __EFMigrationsHistory    TABLE     �   CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);
 +   DROP TABLE public."__EFMigrationsHistory";
       public         heap    postgres    false            %          0    16451    Channels 
   TABLE DATA           �   COPY public."Channels" ("Id", "Name", "Description", "CreationDate", "CreatedBy", "LastUpdateDate", "LastUpdatedBy") FROM stdin;
    public          postgres    false    217   V4       '          0    16459    Projects 
   TABLE DATA           �   COPY public."Projects" ("Id", "Name", "Status", "Description", "Active", "CreationDate", "CreatedBy", "LastUpdateDate", "LastUpdatedBy", "ChannelId", "NextAction", "PIC", "Progress") FROM stdin;
    public          postgres    false    219   �4       )          0    16467    Roles 
   TABLE DATA           F   COPY public."Roles" ("Id", "Code", "Name", "Description") FROM stdin;
    public          postgres    false    221   �5       /          0    16516    Settings 
   TABLE DATA           D   COPY public."Settings" ("Id", "UserId", "Key", "Value") FROM stdin;
    public          postgres    false    227   (6       +          0    16475    Tasks 
   TABLE DATA           !  COPY public."Tasks" ("Id", "ProjectId", "ProjectName", "Subject", "Description", "HelpdeskTicket", "Status", "Progress", "PIC", "Requestor", "Assignee", "StartDate", "DueDate", "Active", "CreationDate", "CreatedBy", "LastUpdateDate", "LastUpdatedBy", "EstimateDate", "Action") FROM stdin;
    public          postgres    false    223   �6       -          0    16483    Users 
   TABLE DATA           �   COPY public."Users" ("Id", "RoleId", "Username", "Password", "DisplayName", "Fullname", "Email", "Phone", "Active", "ChannelId", "ChannelName", "LastLogin", "CreationDate", "CreatedBy", "LastUpdateDate", "LastUpdatedBy") FROM stdin;
    public          postgres    false    225   �9       #          0    16445    __EFMigrationsHistory 
   TABLE DATA           R   COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
    public          postgres    false    215   2;       7           0    0    Channels_Id_seq    SEQUENCE SET     @   SELECT pg_catalog.setval('public."Channels_Id_seq"', 1, false);
          public          postgres    false    216            8           0    0    Projects_Id_seq    SEQUENCE SET     @   SELECT pg_catalog.setval('public."Projects_Id_seq"', 49, true);
          public          postgres    false    218            9           0    0    Roles_Id_seq    SEQUENCE SET     =   SELECT pg_catalog.setval('public."Roles_Id_seq"', 1, false);
          public          postgres    false    220            :           0    0    Settings_Id_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public."Settings_Id_seq"', 5, true);
          public          postgres    false    226            ;           0    0    Tasks_Id_seq    SEQUENCE SET     =   SELECT pg_catalog.setval('public."Tasks_Id_seq"', 72, true);
          public          postgres    false    222            <           0    0    Users_Id_seq    SEQUENCE SET     =   SELECT pg_catalog.setval('public."Users_Id_seq"', 12, true);
          public          postgres    false    224            �           2606    16457    Channels PK_Channels 
   CONSTRAINT     X   ALTER TABLE ONLY public."Channels"
    ADD CONSTRAINT "PK_Channels" PRIMARY KEY ("Id");
 B   ALTER TABLE ONLY public."Channels" DROP CONSTRAINT "PK_Channels";
       public            postgres    false    217            �           2606    16465    Projects PK_Projects 
   CONSTRAINT     X   ALTER TABLE ONLY public."Projects"
    ADD CONSTRAINT "PK_Projects" PRIMARY KEY ("Id");
 B   ALTER TABLE ONLY public."Projects" DROP CONSTRAINT "PK_Projects";
       public            postgres    false    219            �           2606    16473    Roles PK_Roles 
   CONSTRAINT     R   ALTER TABLE ONLY public."Roles"
    ADD CONSTRAINT "PK_Roles" PRIMARY KEY ("Id");
 <   ALTER TABLE ONLY public."Roles" DROP CONSTRAINT "PK_Roles";
       public            postgres    false    221            �           2606    16522    Settings PK_Settings 
   CONSTRAINT     X   ALTER TABLE ONLY public."Settings"
    ADD CONSTRAINT "PK_Settings" PRIMARY KEY ("Id");
 B   ALTER TABLE ONLY public."Settings" DROP CONSTRAINT "PK_Settings";
       public            postgres    false    227            �           2606    16481    Tasks PK_Tasks 
   CONSTRAINT     R   ALTER TABLE ONLY public."Tasks"
    ADD CONSTRAINT "PK_Tasks" PRIMARY KEY ("Id");
 <   ALTER TABLE ONLY public."Tasks" DROP CONSTRAINT "PK_Tasks";
       public            postgres    false    223            �           2606    16489    Users PK_Users 
   CONSTRAINT     R   ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "PK_Users" PRIMARY KEY ("Id");
 <   ALTER TABLE ONLY public."Users" DROP CONSTRAINT "PK_Users";
       public            postgres    false    225            �           2606    16449 .   __EFMigrationsHistory PK___EFMigrationsHistory 
   CONSTRAINT     {   ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");
 \   ALTER TABLE ONLY public."__EFMigrationsHistory" DROP CONSTRAINT "PK___EFMigrationsHistory";
       public            postgres    false    215            �           1259    16495    IX_Users_RoleId    INDEX     I   CREATE INDEX "IX_Users_RoleId" ON public."Users" USING btree ("RoleId");
 %   DROP INDEX public."IX_Users_RoleId";
       public            postgres    false    225            �           2606    16490    Users FK_Users_Roles_RoleId    FK CONSTRAINT     �   ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "FK_Users_Roles_RoleId" FOREIGN KEY ("RoleId") REFERENCES public."Roles"("Id") ON DELETE CASCADE;
 I   ALTER TABLE ONLY public."Users" DROP CONSTRAINT "FK_Users_Roles_RoleId";
       public          postgres    false    221    225    3212            %      x�3��/JL�I�Q1~p����� ��n      '   S  x�u�Mo�0���e�F;Ii��l��
� .\�W�`-*�����XE��<z_[��g�����s�
 N�ͽ6W=-Q� ���ː��4*�%_*�z/���-I��ϝ�9|���Q��ø7��a����k�`t�Ods������|��R6���l:�<�#��+��JZtl����4ص9U���+�OF��@ɐk%5w��RK�����.Z�\n!īrˮ��`�,-�*;dy�uǛ�b��|�t[K��'yV��v�B�<YޓR�����S+��Ӟ���$�<pj\8ة�ǐ����N긲t�ԏ�Ec���l,�@�iiR%�y����¶,��S��      )   4   x�3�tt����tL��̃�\F�.�a�>��A�.�e�9��EW� ��      /   a   x�3�4�qt�q���wq���/�M��2��y���qV+�g�&�(YE+[*�(*��(9g�祦*Y����(�$g�$��CDj�b���� �a9      +   A  x�͖[o�:ǟ�O!���6G$u��N���m�=���M��gKl8�.�~R�N�زD���ʚ�����̧��G��Џ������������ivZ����{ O��=C��L�gB���f��p �ȑ�A%Z+��r$�vJŤIkٞ4�dR���t����'��c>�'���!�����>S��:X|,��#A+�E<�	'����N�6V	��$��x��"&%{��؛j�59Yf|P����f׫��@	����.����7����^1��H��z�j��g|���&�!��~�VY�e`R��:�L���'ï�,e�����q�q�;b��{[�yb�b
1Y�=�"R�
����k��o�������ao��br˧�8�/��S~S�c��'���n',o٘���rL�d����As��.o�l�2�^�-��룽���O�}]�=������&���H�z崊Ws�蔍QA�fSi���:F��uq[L�����+�Q^�b\�u�o��c>⟳)Ͽ������ם�����^���z-��¼��w���I�Xy-{�ʡ�J$Io� ��D����#�0�3��<��(�1o�IkP�v�0����i~��u� ���i�U���TG���=Qt@��-&�L��^o�Yw�\4w|�7yu�M�|��~��g�t�{�j?����s�Z�M~o�HƉ�Fm�}W��
�,��]�E��"{��N`�X
	�n��:) IH�U�F�UT��6:�� ���$
@oA����9�d�K�c ��~YS�Gh��
����Y+���U[Y_�Q}�ն�      -   8  x���?j�0�g��@�'ɲ"M�.��t��c�$�	v!(�F�R�ܥ���&�����������ώCUw{:����j�[�v��Y�)�_�Qgˀ����M�nK��n��E�-v�����?�C��κ���	�	`\������3ІE�+I�Gs�Q�evC�#Y�ݡ�n<_�l8=Z�mh�(�Y���n{xr��k#���s��!,T!�MF�(��WKҪNO����������+�`���ĵ˕��a��,V����$\pu _.>S]4I���|�����:s#�;P��_�(���S�1      #   �   x�m��
!��s>��8c�q/A� X�"*��hl�?�`�����+P4�B w)�e�^���k9�D��P��f��j�C4�d���Z�ey¢"�fS����a������_ѷ�O3��-�:�m��B���B| �;A�     